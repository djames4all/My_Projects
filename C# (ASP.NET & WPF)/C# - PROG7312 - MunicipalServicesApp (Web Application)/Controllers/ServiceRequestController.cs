using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.DataStructures;
using MunicipalServicesApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MunicipalServicesApp.Controllers
{
    public class ServiceRequestController : Controller
    {
        private static readonly List<ServiceRequest> _requests = new();

        private static readonly BinarySearchTree<ServiceRequest> _bst = new BinarySearchTree<ServiceRequest>(r => r.Id);
        private static readonly AVLTree<ServiceRequest> _avl = new AVLTree<ServiceRequest>(r => r.Id);
        private static readonly RedBlackTree<ServiceRequest> _rbt = new RedBlackTree<ServiceRequest>();
        private static readonly MinHeap _heap = new MinHeap();
        private static readonly Graph _graph = new Graph();

        private readonly IWebHostEnvironment _env;

        public ServiceRequestController(IWebHostEnvironment env)
        {
            _env = env;
            if (!_requests.Any()) SeedSample();
        }

        private void SeedSample()
        {
            var s1 = new ServiceRequest { Id = 1, Title = "Water Leakage", Description = "Pipe burst on Main St", Location = "Ward 2", Priority = 4, Status = "Pending", SubmittedAt = DateTime.Now.AddHours(-30) };
            var s2 = new ServiceRequest { Id = 2, Title = "Street Light Outage", Description = "Lamp post 54 not working", Location = "Ward 5", Priority = 2, Status = "In Progress", SubmittedAt = DateTime.Now.AddHours(-20) };
            var s3 = new ServiceRequest { Id = 3, Title = "Pothole", Description = "Large pothole near school", Location = "Ward 7", Priority = 1, Status = "Resolved", SubmittedAt = DateTime.Now.AddDays(-2) };
            var s4 = new ServiceRequest { Id = 4, Title = "Storm Drain Blockage", Description = "Drain overflowing", Location = "Ward 9", Priority = 3, Status = "Pending", SubmittedAt = DateTime.Now.AddHours(-10) };
            var s5 = new ServiceRequest { Id = 5, Title = "Illegal Dumping", Description = "Rubbish at corner", Location = "Ward 10", Priority = 2, Status = "In Progress", SubmittedAt = DateTime.Now.AddHours(-5) };

            AddRequestToAllStructures(s1);
            AddRequestToAllStructures(s2);
            AddRequestToAllStructures(s3);
            AddRequestToAllStructures(s4);
            AddRequestToAllStructures(s5);

            _graph.AddEdge(s1.TrackingId, s2.TrackingId, 2);
            _graph.AddEdge(s2.TrackingId, s3.TrackingId, 3);
            _graph.AddEdge(s3.TrackingId, s4.TrackingId, 4);
            _graph.AddEdge(s4.TrackingId, s5.TrackingId, 1);
        }

        private void AddRequestToAllStructures(ServiceRequest r)
        {
            _requests.Add(r);
            _bst.Insert(r);
            _avl.Insert(r);
            _rbt.Insert(r);
            _heap.Insert(r);
            _graph.AddVertex(r.TrackingId);
        }

        public IActionResult Create()
        {
            ViewBag.Requests = _requests.OrderByDescending(x => x.SubmittedAt).ToList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ServiceRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Requests = _requests.OrderByDescending(x => x.SubmittedAt).ToList();
                return View(model);
            }

            if (model.MediaFile != null && model.MediaFile.Length > 0)
            {
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
                var ext = Path.GetExtension(model.MediaFile.FileName).ToLowerInvariant();
                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("MediaFile", "Invalid file type.");
                    ViewBag.Requests = _requests.OrderByDescending(x => x.SubmittedAt).ToList();
                    return View(model);
                }

                const long maxSize = 5 * 1024 * 1024;
                if (model.MediaFile.Length > maxSize)
                {
                    ModelState.AddModelError("MediaFile", "File too large (max 5MB).");
                    ViewBag.Requests = _requests.OrderByDescending(x => x.SubmittedAt).ToList();
                    return View(model);
                }

                var uploadDir = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);
                var unique = Guid.NewGuid().ToString() + ext;
                var fullPath = Path.Combine(uploadDir, unique);
                using (var fs = new FileStream(fullPath, FileMode.Create)) model.MediaFile.CopyTo(fs);
                model.MediaFilePath = "/uploads/" + unique;
            }

            model.Id = (_requests.Count > 0) ? _requests.Max(x => x.Id) + 1 : 1;
            model.SubmittedAt = DateTime.Now;
            if (string.IsNullOrWhiteSpace(model.TrackingId)) model.TrackingId = Guid.NewGuid().ToString();

            AddRequestToAllStructures(model);

            if (!string.IsNullOrWhiteSpace(model.RelatedTrackingIds))
            {
                var toks = model.RelatedTrackingIds.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var t in toks) _graph.AddEdge(model.TrackingId, t, 1.0);
            }

            ViewBag.SuccessMessage = "Request submitted successfully!";
            ModelState.Clear();
            ViewBag.Requests = _requests.OrderByDescending(x => x.SubmittedAt).ToList();
            return View();
        }

        public IActionResult Index()
        {
            var all = _requests.OrderByDescending(x => x.SubmittedAt).ToList();
            var byPriority = _heap.ToSortedList();
            var bstList = _bst.InOrder();
            var avlList = _avl.InOrder();
            var rbtJsonObj = _rbt.ExportAsJson();
            var rbtJson = JsonSerializer.Serialize(rbtJsonObj, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var mst = new List<(string, string, double)>();
            if (_requests.Any()) mst = _graph.PrimMST(_requests.First().TrackingId);

            ViewBag.All = all;
            ViewBag.ByPriority = byPriority;
            ViewBag.ByBST = bstList;
            ViewBag.ByAVL = avlList;
            ViewBag.MST = mst;
            ViewBag.RedBlackJson = rbtJson;
            ViewBag.RedBlackCount = _rbt.CountNodes();
            ViewBag.RedBlackHeight = _rbt.GetHeight();
            ViewBag.HeapMin = _heap.Peek();

            return View();
        }

        public IActionResult Track() => View();

        [HttpPost]
        public IActionResult TrackResult(string trackingId)
        {
            if (string.IsNullOrWhiteSpace(trackingId))
            {
                ViewBag.Message = "Please provide a tracking id.";
                return View("Track");
            }

            var found = _requests.FirstOrDefault(r => r.TrackingId.Equals(trackingId.Trim(), StringComparison.OrdinalIgnoreCase));
            if (found == null)
            {
                ViewBag.Message = $"No request found for tracking id {trackingId}.";
                return View("Track");
            }

            ViewBag.BFS = _graph.BFS(found.TrackingId);
            ViewBag.DFS = _graph.DFS(found.TrackingId);
            ViewBag.MST = _graph.PrimMST(found.TrackingId);
            return View("Details", found);
        }

        public IActionResult Details(int id)
        {
            var found = _requests.FirstOrDefault(r => r.Id == id);
            if (found == null) return NotFound();
            ViewBag.BFS = _graph.BFS(found.TrackingId);
            ViewBag.DFS = _graph.DFS(found.TrackingId);
            ViewBag.MST = _graph.PrimMST(found.TrackingId);
            return View(found);
        }

        [HttpPost]
        public IActionResult UpdateStatusAjax(string trackingId, string status)
        {
            var req = _requests.FirstOrDefault(r => r.TrackingId == trackingId);
            if (req == null) return Json(new { success = false, message = "Request not found." });
            var allowed = new[] { "Pending", "In Progress", "Resolved" };
            if (!allowed.Contains(status)) return Json(new { success = false, message = "Invalid status." });
            req.Status = status;
            return Json(new { success = true, newStatus = status });
        }
    }
}
