
Student Tutoring Application
A role-based tutoring app built with ASP.NET Core MVC and SQL Server.
This was a group project created for an NBCC server-side web development course.

What it does
The app helps manage tutoring by letting:
Students request tutoring
Tutors set their availability
Admins manage the overall process

Tech used
ASP.NET Core MVC (C#)
Entity Framework Core
SQL Server
ASP.NET Identity
Razor Views + Bootstrap

What I worked on
I was mainly responsible for the Tutor side of the application.
I built the tutor availability flow from start to finish, including the form, validation rules, confirmation step, and saving data to the database. I also connected tutor availability to the logged-in user and handled duplicate time-slot checks.
Running the project
Open the solution in Visual Studio
Update the SQL Server connection string
Run the app using IIS Express

Notes
This project focuses on backend logic and workflow design rather than deployment.



Original：
# PROG1342A-TutoringApplication
PROG1342A Server-Side Web: MVC Group Project

Product summary/background

New Brunswick Community College (NBCC) is a distinguished public
post-secondary institution located in New Brunswick, Canada, renowned for its
commitment to academic excellence. Operating across six campuses, NBCC
offers a diverse range of programs.

Students at NBCC benefit from a dynamic learning environment characterized
by hands-on training, cutting-edge equipment, and immersive real-world
experiences. With a portfolio of over 90 programs, the college provides
comprehensive learning opportunities, empowering students to enter the
workforce with expertise and confidence.

NBCC aims to develop a prototype tutoring system in its ongoing commitment
to student success. This innovative system will allow students needing tutoring
services to seamlessly book sessions with qualified tutors. The proposed plan
all students.

Currently, students request online tutoring via email to the Student Services
Department, which then notifies the Learning Strategy team. The Learning
Strategy team sources a tutor and communicates with Student Services, who
then inform the student about the tutor's availability. This process involves
numerous back-and-forth emails and is cumbersome. As a solution, NBCC
intends to replace the existing manual tutoring scheduling process with an
automated process.

Business goals/objectives
  1. To improve efficiency and effectiveness in tutoring operations in NBCC.
  2. To enhance the overall tutoring experience for NBCC students.
  3. To develop a web-based tutoring system to streamline and simplify the tutor request process.
