# Health Catalyst - The People Search Sample Application
People search sample application for Health Catalyst Senior Software Engineer using Dot Net Core Web API, Aurelia, and Entity Framework

# Usage Instructions:
Clone Repository  
Within the project navigate to the directory Dist  
**Execute VanderStackHealthCatalyst.exe**  
or  
:\\> dotnet run  
Open your web browser and navigate to http://localhost:5000  
  
    
Notes: Development took place in VS2019 Community Preview, should be backward compatible with VS2017  
  
# Test Instructions:  
:\\> dotnet test  
  
# Deployment Instructions:
:\\> dotnet publish -c release -r win10-x64  

# Business Requirements

The application accepts search input in a text box and then displays in a pleasing style a list of people where any part of their first or last name matches what was typed in the search box (displaying at least name, address, age, interests, and a picture).  
Solution should either seed data or provide a way to enter new users or both.  
Simulate search being slow and have the UI gracefully handle the delay.  
  
Filtering People by partial match to first or last name:  
![Filtering People](https://raw.githubusercontent.com/vanderstack/VanderStackHealthCatalyst/master/HealthCatalystPeopleSearch/static/Demo/filter.gif)  
  
Seed Person Zachary:  
![Seed Person](https://raw.githubusercontent.com/vanderstack/VanderStackHealthCatalyst/master/HealthCatalystPeopleSearch/static/Demo/seed-zach.gif)  
  
Seed Person Patrick (Zach's Dog):  
![Seed Dog](https://raw.githubusercontent.com/vanderstack/VanderStackHealthCatalyst/master/HealthCatalystPeopleSearch/static/Demo/seed-patrick.gif)  
  
Gracefully Handle Latency:  
![Gracefully Handle Latency](https://raw.githubusercontent.com/vanderstack/VanderStackHealthCatalyst/master/HealthCatalystPeopleSearch/static/Demo/latency.gif)  
  
**Bonus Feature**: Demonstrate Basic Validation:  
![Seed Data with Validation Errors](https://raw.githubusercontent.com/vanderstack/VanderStackHealthCatalyst/master/HealthCatalystPeopleSearch/static/Demo/validation.gif)  
  
# Technical Requirements

A Web Application using WebAPI and a front-end JavaScript framework (e.g., Angular, AngularJS, React, Aurelia, etc.).  
Use an ORM framework to talk to the database.  
Unit Tests for appropriate parts of the application.  
  
Unit Tests Passing:  
![Successful Tests](https://raw.githubusercontent.com/vanderstack/VanderStackHealthCatalyst/master/HealthCatalystPeopleSearch/static/Demo/tests.gif)  
  
# Assignment Background

Overview:
At Catalyst the process begins with a coding problem that I will be giving you in this email.  It is meant to assess your competency with .NET, the technologies we use every day, and with software engineering principles and design patterns. As such, it is a key component in our interviewing and hiring process.  Please use this as a chance to show off your best work. After completing it, we will review your solution and consider you for the next step in the process, which is an onsite interview.  Best of luck! We look forward to your solution!

Coding Problem:
Our preference is that you do your development in GitHub, so that we can clone your solution directly from there. Please ensure that cloning the repository, then running it directly (without any additional setup steps) will start the app properly. We consider this a reflection of how you work. If the application does not run, we will not be able to grade it.