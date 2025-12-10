#Municipal Services App (README File)
Version: 3.0
Author: Daniel Luke James – ST10393280
Date: 12/11/2025

------------------------------------------------------------
#Links
------------------------------------------------------------
1. GitHub Repository: https://github.com/VCSTDN2024/prog7312-poe-ST10393280.git
2. Demonstration Video: https://youtu.be/wkgzNE-NvMA

------------------------------------------------------------
#Overview
------------------------------------------------------------
The Municipal Services Application is a modern, web-based and desktop-ready platform designed to empower citizens to actively participate in improving local government service delivery. It allows residents to report issues, participate in surveys, submit improvement suggestions, and view local municipal announcements and events.

The application embodies Participatory Co-Creation principles, where citizens not only report and monitor municipal services but also contribute feedback that informs future enhancements to the platform and municipal operations. Through secure communication channels, data validation, and structured reporting, the system establishes a two-way communication link between residents and municipal staff, promoting accountability, responsiveness, and transparency.

The platform’s design emphasizes usability, responsiveness, and accessibility, providing a cohesive user experience across desktop, tablet, and mobile devices. The entire system is underpinned by carefully implemented custom data structures and algorithms that enhance responsiveness, minimize lookup times, and improve system scalability — especially within the “Service Request Status” and “Local Events” modules.

------------------------------------------------------------
#Features
------------------------------------------------------------
1. Report Issues: Submit detailed reports about municipal problems such as water leaks, power outages, sanitation breakdowns, or road damage.
2. Community Survey: Participate in community-wide surveys to provide input on service delivery effectiveness and citizen satisfaction.
3. Report a Suggestion: Share constructive ideas or feature improvement requests to enhance application functionality or public services.
4. Community Feedback: Browse summarized reports, suggestions, and public sentiment gathered from other users.
5. File Uploads: Attach supporting documents or images (e.g., photos of potholes, broken pipes) to aid municipal teams.
6. Responsive Design: Works seamlessly on desktop, tablet, and mobile devices.
7. Participatory Co-Creation: Residents can influence future features and contribute to improving both service delivery and digital governance.
8. Local Events & Announcements: View upcoming consultations, workshops, or local festivals. Includes intelligent search, sorting, and smart recommendation features.
9. Personal Calendar: Add municipal events to a personal session-based calendar that can be sorted, viewed, or cleared dynamically.
10. Service Request Status: Track submitted service requests, monitor progress updates, and view estimated completion times (implemented in Phase 3).

------------------------------------------------------------
#Technology Stack
------------------------------------------------------------
Backend: ASP.NET Core MVC (C#)
Frontend: Razor Pages, Bootstrap 5, JavaScript, jQuery
Database (Future Integration): Microsoft SQL Server / Firebase (for persistent storage)
Development Tools: Visual Studio 2022, Visual Studio Code
Version Control: Git and GitHub
Algorithmic Structures: Custom implementations of Queues, Stacks, Priority Queues, Hash Tables, Dictionaries, Sorted Dictionaries, Sets, Binary Search Trees, AVL Trees, and Graphs

------------------------------------------------------------
#Installation Instructions
------------------------------------------------------------
1. Clone the Repository:
   git clone https://github.com/VCSTDN2024/prog7312-poe-ST10393280.git

2. Open the Solution in Visual Studio:
   - Launch Visual Studio 2022.
   - Open the file MunicipalServicesApp.sln.

3. Restore Dependencies:
   - Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution.
   - Restore all required packages (Bootstrap, Newtonsoft.Json, etc.).

4. Set the Startup Project:
   - Right-click on MunicipalServicesApp → Set as Startup Project.

5. Build and Run the Application:
   - Select the build configuration as Debug or Release.
   - Click Build → Rebuild Solution.
   - Press F5 to run the project (HTTPS recommended).

------------------------------------------------------------
#Using the Application
------------------------------------------------------------
##Home Page:
The homepage acts as the central dashboard, displaying navigation links to the main modules:
- Report Issues
- Community Engagement
- Local Events and Announcements
- Service Request Status

##Report Issues Page:
1. Click on “Report Issues” from the home screen.
2. Enter Location, Category, and Description of the issue.
3. Attach a file or image if relevant.
4. Click Submit — a confirmation modal ensures large file uploads are intentional.
5. The issue is then queued for municipal processing, with a unique Request ID generated automatically.

##Community Engagement Page:
1. Select “Community Engagement.”
2. Community Survey: Complete multiple-choice or open-ended survey questions.
3. Report a Suggestion: Submit constructive proposals, optionally including your name.
4. Community Feedback: Review aggregated feedback and statistics from other residents.

##Local Events and Announcements Page:
1. Access via the Events button.
2. Filter by Category, Date Range, or Keyword.
3. View filtered results, or the default list of upcoming events if no matches are found.
4. Add selected events to your Personal Calendar (stored temporarily in session).
5. The Smart Recommendation Engine analyzes:
   - Category preferences
   - Search frequency
   - Event popularity scores
   - Recent user interactions

##Personal Calendar Page:
1. Access your calendar via the Calendar button.
2. All added events are automatically sorted by date and title using a Sorted Dictionary.
3. Users may remove events individually, after which the session data structure updates in real-time.
4. The session-based persistence ensures your calendar remains active until the session expires.

##Service Request Status (Phase 3 Implementation):
This feature enables users to track ongoing requests and their municipal processing stages. It integrates multiple custom data structures to achieve high performance and accuracy:
- Binary Search Tree (BST): Stores all service requests indexed by unique ID, enabling logarithmic-time search and retrieval.
- AVL Tree (Balanced BST): Ensures balanced growth and consistent search performance.
- Hash Table: Provides constant-time lookup of service requests using hash keys.
- Min-Heap: Prioritizes requests based on urgency, ensuring critical issues are processed first.
- Graph (Adjacency List): Represents task dependencies and supports BFS/DFS traversal.
- Queue and Stack: Queues maintain processing order, while stacks enable undo/rollback functionality.
- Minimum Spanning Tree (MST): Optimizes routing for field workers to minimize distance and fuel usage.

------------------------------------------------------------
#Algorithmic & Data Structure Highlights
------------------------------------------------------------
Stacks: Track recently viewed events and maintain undo history.
Queues: Manage submission order and event queues.
Priority Queues: Rank events or service tasks by urgency or popularity.
Dictionaries and Sorted Dictionaries: Enable efficient key-based event lookups.
HashSets: Guarantee uniqueness of categories and tags.
Graphs: Represent dependencies between service tasks.
Binary Trees & AVL Trees: Support ordered storage and balanced data access.
Hash Tables: Enable instant lookups of service request IDs.

------------------------------------------------------------
#Changelog (Version 3.0 Updates)
------------------------------------------------------------
Based on Lecturer Feedback (Parts 1 & 2):
- Expanded README for detailed compilation, installation, and usage instructions.
- Improved UI consistency and responsiveness across multiple devices.
- Replaced all inbuilt C# data structures with custom implementations.
- Optimized recommendation engine for better contextual relevance.
- Enhanced user feedback mechanisms (progress bars, modals, alerts).
- Improved error handling and validation for submissions.
- Applied standardized color palette and accessible font design.

------------------------------------------------------------
#System Requirements
------------------------------------------------------------
Operating System: Windows 10 or later
IDE: Visual Studio 2022 (or VS Code with .NET SDK)
Framework: .NET 6 or .NET Framework 4.8
Browser Support: Chrome, Edge, Firefox
RAM: Minimum 4GB
Disk Space: Minimum 1GB

------------------------------------------------------------
#Future Enhancements
------------------------------------------------------------
- Database Integration (SQL Server / Firebase)
- Web API layer using ASP.NET Core
- Cloud Deployment (Azure / AWS)
- GIS Integration with Google Maps API
- Machine Learning Analytics for predictive maintenance

------------------------------------------------------------
#Acknowledgements
------------------------------------------------------------
Special thanks to the lecturer for valuable feedback and continuous guidance throughout the PROG7312 module.

------------------------------------------------------------
#License
------------------------------------------------------------
This project is licensed under the MIT License for educational and non-commercial use.

End of README File – Version 3.0
