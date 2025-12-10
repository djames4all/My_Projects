using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.Models
{
    public static class EventRepository
    {
        // Main list storing all events
        private static readonly List<EventModel> _events = new();

        // Hash table: Key = Event ID, Value = EventModel
        // Allows O(1) lookup of events by ID
        private static readonly Dictionary<int, EventModel> _eventById = new();

        // SortedDictionary: Key = Event date, Value = List of events on that date
        // Automatically sorted by date for calendar and date-based searches
        private static readonly SortedDictionary<DateTime, List<EventModel>> _eventsByDate = new();

        // HashSet to store unique categories
        // Useful for dropdowns or filters without duplicates
        private static readonly HashSet<string> _categories = new(StringComparer.OrdinalIgnoreCase);

        // Queue: Stores upcoming events in chronological order
        // Used by GetUpcoming() to quickly get next scheduled events
        private static readonly Queue<EventModel> _upcomingQueue = new();

        // Stack: Stores IDs of recently viewed events (Last-In-First-Out)
        // Updated in MarkViewed() and accessed via GetRecentlyViewed()
        private static readonly Stack<int> _recentlyViewed = new();

        // PriorityQueue: Stores events with a computed score for recommendation
        // Events with higher priority for user are dequeued first in Recommend()
        private static readonly PriorityQueue<EventModel, int> _recommendationQueue = new();

        // Dictionary: Key = normalized keyword, Value = List of Event IDs
        // Used for fast search by title, category, or tags
        private static readonly Dictionary<string, List<int>> _keywordIndex = new(StringComparer.OrdinalIgnoreCase);

        // Dictionary: Key = normalized keyword, Value = search count
        // Tracks frequency of searches to improve recommendations
        private static readonly Dictionary<string, int> _searchCounts = new(StringComparer.OrdinalIgnoreCase);

        // Lock object to ensure thread safety
        private static readonly object _lock = new();

        // Static constructor to seed initial events and build indexes
        static EventRepository()
        {
            SeedInitialEvents();
            BuildIndexes();
        }

        #region Seed & Indexing
        private static void SeedInitialEvents()
        {
            // Adding initial demo events
            AddEvent(new EventModel
            {
                Title = "Community Clean-up Drive",
                Description = "Join volunteers from across the municipality to clean parks, playgrounds and sidewalks. Gloves and bags are provided; light refreshments after the cleanup.",
                StartDate = DateTime.Today.AddDays(3).AddHours(9),
                EndDate = DateTime.Today.AddDays(3).AddHours(12),
                Category = "Community",
                Location = "Town Park, Main Road",
                Priority = 3,
                Tags = new List<string> { "cleanup", "community", "volunteer", "environment" },
                ImageUrl = "/images/cleanup.jpg",
                IsFeatured = true
            });

            AddEvent(new EventModel
            {
                Title = "Public Consultation: Roadworks on Main Street",
                Description = "Drop-in session for residents to view proposed plans and ask questions about upcoming roadworks on Main Street and surrounding areas.",
                StartDate = DateTime.Today.AddDays(7).AddHours(18),
                Category = "Consultation",
                Location = "Municipal Hall - Auditorium",
                Priority = 2,
                Tags = new List<string> { "roadworks", "consultation", "transport" },
                ImageUrl = "/images/townhall.jpg"
            });

            AddEvent(new EventModel
            {
                Title = "Arts & Culture Festival",
                Description = "A full-day celebration of local artists, musicians, theatre and food. Family-friendly activities, artisan stalls and live performances all day.",
                StartDate = DateTime.Today.AddDays(21).AddHours(10),
                EndDate = DateTime.Today.AddDays(21).AddHours(21),
                Category = "Festival",
                Location = "City Square",
                Priority = 5,
                Tags = new List<string> { "festival", "culture", "music", "art" },
                ImageUrl = "/images/festival1.jpg",
                IsFeatured = true
            });

            AddEvent(new EventModel
            {
                Title = "Water Conservation Workshop",
                Description = "Practical tips and demonstrations for saving water at home. Hosted by the municipal water department.",
                StartDate = DateTime.Today.AddDays(2).AddHours(14),
                Category = "Workshop",
                Location = "Library Conference Room",
                Priority = 4,
                Tags = new List<string> { "water", "conservation", "workshop" },
                ImageUrl = "/images/water-workshop.jpg"
            });

            AddEvent(new EventModel
            {
                Title = "Youth Coding & Careers Session",
                Description = "Intro to coding and career pathways in technology for learners aged 13–19. Bring your laptop if you have one—limited devices available.",
                StartDate = DateTime.Today.AddDays(10).AddHours(16),
                Category = "Youth",
                Location = "Tech Hub",
                Priority = 6,
                Tags = new List<string> { "youth", "coding", "careers", "education" },
                ImageUrl = "/images/coding.jpg"
            });
        }

        private static void BuildIndexes()
        {
            lock (_lock)
            {
                // Clear all structures before rebuilding
                _eventById.Clear();
                _eventsByDate.Clear();
                _categories.Clear();
                _keywordIndex.Clear();

                _upcomingQueue.Clear();
                while (_recommendationQueue.Count > 0) _recommendationQueue.TryDequeue(out _, out _);

                foreach (var ev in _events)
                {
                    // Add to hash table
                    _eventById[ev.Id] = ev;

                    // Add to SortedDictionary by date
                    var dateKey = ev.StartDate.Date;
                    if (!_eventsByDate.ContainsKey(dateKey)) _eventsByDate[dateKey] = new List<EventModel>();
                    _eventsByDate[dateKey].Add(ev);

                    // Add unique category
                    if (!string.IsNullOrWhiteSpace(ev.Category)) _categories.Add(ev.Category);

                    // Enqueue for upcoming events
                    _upcomingQueue.Enqueue(ev);

                    // Index keywords for search
                    IndexKeywordsForEvent(ev);
                }
            }
        }

        private static void IndexKeywordsForEvent(EventModel ev)
        {
            var tokens = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Index title words
            if (!string.IsNullOrWhiteSpace(ev.Title))
            {
                foreach (var tok in ev.Title.Split(new[] { ' ', ',', '.', ';', ':', '-', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries))
                    tokens.Add(Normalize(tok));
            }

            // Index category
            if (!string.IsNullOrWhiteSpace(ev.Category)) tokens.Add(Normalize(ev.Category));

            // Index tags
            foreach (var t in ev.Tags ?? new List<string>()) tokens.Add(Normalize(t));

            // Add to keyword index dictionary
            foreach (var tok in tokens)
            {
                if (!_keywordIndex.ContainsKey(tok)) _keywordIndex[tok] = new List<int>();
                if (!_keywordIndex[tok].Contains(ev.Id)) _keywordIndex[tok].Add(ev.Id);
            }
        }
        #endregion

        #region Utilities
        private static string Normalize(string s) => s?.Trim().ToLowerInvariant() ?? string.Empty;

        private static int NextId()
        {
            lock (_lock) return _events.Count == 0 ? 1 : _events.Max(e => e.Id) + 1;
        }
        #endregion

        #region CRUD + Queries
        public static IEnumerable<EventModel> GetAllEvents()
        {
            lock (_lock) return _events.OrderBy(e => e.StartDate).ToList();
        }

        public static IEnumerable<EventModel> GetUpcoming(int max = 10)
        {
            // Uses Queue<EventModel> to retrieve upcoming events efficiently
            lock (_lock) return _upcomingQueue.Take(max).OrderBy(e => e.StartDate).ToList();
        }

        public static EventModel? GetById(int id)
        {
            // Uses Dictionary<int, EventModel> for O(1) lookup
            lock (_lock)
            {
                return _eventById.TryGetValue(id, out var ev) ? ev : null;
            }
        }

        public static IEnumerable<EventModel> GetByDate(DateTime date)
        {
            // Uses SortedDictionary<DateTime, List<EventModel>> to retrieve events by date
            lock (_lock)
            {
                return _eventsByDate.TryGetValue(date.Date, out var list) ? list.OrderBy(e => e.StartDate).ToList() : Enumerable.Empty<EventModel>();
            }
        }

        public static IEnumerable<string> GetCategories()
        {
            // Uses HashSet<string> to get unique categories
            lock (_lock) return _categories.OrderBy(c => c).ToList();
        }

        public static void AddEvent(EventModel ev)
        {
            lock (_lock)
            {
                ev.Id = NextId();
                _events.Add(ev);
                _eventById[ev.Id] = ev;

                var dateKey = ev.StartDate.Date;
                if (!_eventsByDate.ContainsKey(dateKey)) _eventsByDate[dateKey] = new List<EventModel>();
                _eventsByDate[dateKey].Add(ev);

                if (!string.IsNullOrWhiteSpace(ev.Category)) _categories.Add(ev.Category);

                _upcomingQueue.Enqueue(ev);

                IndexKeywordsForEvent(ev);
            }
        }
        #endregion

        #region Search + Analytics
        public static IEnumerable<EventModel> Search(string? category = null, DateTime? from = null, DateTime? to = null, string? keyword = null)
        {
            lock (_lock)
            {
                IEnumerable<EventModel> query = _events;

                if (!string.IsNullOrWhiteSpace(category))
                    query = query.Where(e => string.Equals(e.Category, category, StringComparison.OrdinalIgnoreCase));

                if (from.HasValue) query = query.Where(e => e.StartDate.Date >= from.Value.Date);
                if (to.HasValue) query = query.Where(e => e.StartDate.Date <= to.Value.Date);

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    var tok = Normalize(keyword);

                    if (_keywordIndex.TryGetValue(tok, out var ids))
                    {
                        // Fast lookup using Dictionary<string, List<int>> for search
                        var set = new HashSet<int>(ids);
                        query = query.Where(e => set.Contains(e.Id));
                    }
                    else
                    {
                        // Fallback search in title, description, and tags
                        query = query.Where(e =>
                            (!string.IsNullOrWhiteSpace(e.Title) && e.Title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                            || (!string.IsNullOrWhiteSpace(e.Description) && e.Description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                            || (e.Tags != null && e.Tags.Any(t => t.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0))
                        );
                    }

                    // Track search counts for recommendation scoring
                    foreach (var tk in keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var n = Normalize(tk);
                        if (string.IsNullOrEmpty(n)) continue;
                        if (!_searchCounts.ContainsKey(n)) _searchCounts[n] = 0;
                        _searchCounts[n] += 1;
                    }
                }

                return query.OrderBy(e => e.StartDate).ToList();
            }
        }
        #endregion

        #region Recently Viewed
        public static void MarkViewed(int id)
        {
            // Push to Stack<int> to track recently viewed event IDs
            lock (_lock)
            {
                _recentlyViewed.Push(id);
                while (_recentlyViewed.Count > 50) _recentlyViewed.Pop();
            }
        }

        public static IEnumerable<EventModel> GetRecentlyViewed(int max = 5)
        {
            // Retrieve events from Stack<int> while avoiding duplicates
            lock (_lock)
            {
                var seen = new HashSet<int>();
                var list = new List<EventModel>();
                foreach (var id in _recentlyViewed)
                {
                    if (seen.Contains(id)) continue;
                    if (_eventById.TryGetValue(id, out var ev))
                    {
                        list.Add(ev);
                        seen.Add(id);
                        if (list.Count >= max) break;
                    }
                }
                return list;
            }
        }
        #endregion

        #region Recommendation Engine
        public static IEnumerable<EventModel> Recommend(string? preferredCategory = null, int max = 5)
        {
            lock (_lock)
            {
                // Clear priority queue before computing new recommendations
                while (_recommendationQueue.Count > 0) _recommendationQueue.TryDequeue(out _, out _);

                foreach (var ev in _events)
                {
                    int score = 0;

                    // Score boost if category matches user's preference
                    if (!string.IsNullOrWhiteSpace(preferredCategory) &&
                        string.Equals(ev.Category, preferredCategory, StringComparison.OrdinalIgnoreCase))
                        score += 10;

                    // Score boost based on tag popularity from _searchCounts
                    if (ev.Tags != null)
                    {
                        foreach (var t in ev.Tags)
                        {
                            var tok = Normalize(t);
                            if (_searchCounts.TryGetValue(tok, out var c))
                                score += 5 * Math.Min(c, 5);
                        }
                    }

                    // Score boost based on title keyword searches
                    var titleTokens = (ev.Title ?? string.Empty)
                        .Split(new[] { ' ', ',', '.', ';', ':', '-', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(Normalize);
                    foreach (var tok in titleTokens)
                        if (_searchCounts.TryGetValue(tok, out var c))
                            score += Math.Min(c, 5);

                    // Score boost based on priority (lower number = higher priority)
                    score += Math.Max(0, 10 - ev.Priority);

                    // Additional boost for featured events
                    if (ev.IsFeatured) score += 5;

                    // Add to PriorityQueue with negative score (highest score dequeues first)
                    _recommendationQueue.Enqueue(ev, -score);
                }

                // Dequeue top recommendations
                var result = new List<EventModel>();
                while (_recommendationQueue.Count > 0 && result.Count < max)
                {
                    if (_recommendationQueue.TryDequeue(out var ev, out _))
                        result.Add(ev);
                }
                return result;
            }
        }
        #endregion
    }
}
