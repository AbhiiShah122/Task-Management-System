# Task Management System

- The Task Management System is designed to assist employees in tracking their tasks, attaching documents, adding notes, and marking tasks as complete. 
- It also enables Employee Managers/Team Leaders to monitor the tasks of all their team members. 
- Additionally, Company Admins can generate reports to evaluate team performance based on task completion within specific time frames such as weekly or monthly.

## Features

- **Task Tracking**: Employees can view and manage their tasks.
- **Document Management**: Attach documents to tasks for better clarity and record-keeping.
- **Notes Addition**: Add notes to tasks for additional information or updates.
- **Task Completion**: Mark tasks as completed once done.
- **Team Monitoring**: Managers/Leaders can track the progress of their team members' tasks.
- **Reporting**: Admins can fetch reports on team performance and task completion rates.

## Architecture

The system is built on a multi-layered architecture to ensure separation of concerns, scalability, and maintainability:

- **Data Layer**: Utilizes Entity Framework Core for ORM with SQL Server as the database. The database schema is designed to support tasks, users, roles, documents, and notes.
- **Repository Layer**: Implements the Repository pattern to abstract the data layer and provide a clean API for data access to the upper layers.
- **Service Layer**: Contains business logic and communicates between the repository layer and the controllers. It implements the services required by the application.
- **Controller Layer**: The API layer that interacts with the front end. It receives HTTP requests and returns responses.

## Technology Stack

- **.NET 8.0**: For building the web API.
- **Entity Framework Core**: For data access.
- **SQL Server**: As the database.
- **JWT (JSON Web Tokens)**: For authentication and authorization.
- **Swagger**: For API documentation and testing.

## Design Patterns

- **Repository Pattern**: To decouple the business logic and the data access layers.
- **Dependency Injection**: Used extensively across the project for injecting services, repositories, and other dependencies.

## Database Design

The database for our Task Management System is designed using the **Code First Approach** and created below Tables:

- **Users**: This table stores information about all users in the system. It acts as a base table for different types of users such as Employees, Managers, and Admins, demonstrating the use of inheritance in our database design.
- **Employees**: Inherits from the Users table and stores specific information related to employees.
- **Managers**: Inherits from the Users table and contains information specific to managers.
- **Admins**: Inherits from the Users table, holding data pertinent to system administrators.
- **Tasks**: Stores tasks assigned to users. Each task can be associated with multiple attachments and notes.
- **TaskAttachments**: Represents documents or files attached to a task. This demonstrates a one-to-many relationship between Tasks and TaskAttachments.
- **TaskNotes**: Stores notes added to tasks, illustrating a one-to-many relationship with the Tasks table.
- **Roles**: Contains roles that can be assigned to users, supporting role-based access control within the system.
- **Teams**: Stores information about teams within the organization. Users (Employees, Managers) can be associated with teams.

## API Overview

### AuthenticationController
- **Login**: Handles user authentication.
  - Endpoint: `POST {{TaskManagementSystem_HostAddress}}/api/authentication/login`

### TasksController
Responsible for managing tasks with various endpoints protected by role-based access control. The key operations include:

- **Create Task**: Allows Admin, Employee, and Manager roles to create a new task.
  - Endpoint: `POST {{TaskManagementSystem_HostAddress}}/api/tasks`
  - Access: Admin, Employee, Manager

- **Update Task**: Enables Admin, Employee, and Manager roles to update an existing task.
  - Endpoint: `PUT {{TaskManagementSystem_HostAddress}}/api/tasks/{taskId}`
  - Access: Admin, Employee, Manager

- **Delete Task**: Permits Admin, Employee, and Manager roles to delete a task.
  - Endpoint: `DELETE {{TaskManagementSystem_HostAddress}}/api/tasks/{taskId}`
  - Access: Admin, Employee, Manager

- **Add Attachment**: Allows Admin, Employee, and Manager roles to add an attachment to a task.
  - Endpoint: `POST {{TaskManagementSystem_HostAddress}}/api/tasks/attachments`
  - Access: Admin, Employee, Manager

- **Add Note**: Enables Admin, Employee, and Manager roles to add a note to a task.
  - Endpoint: `POST {{TaskManagementSystem_HostAddress}}/api/tasks/notes`
  - Access: Admin, Employee, Manager

- **Complete Task**: Allows Admin, Employee, and Manager roles to mark a task as complete.
  - Endpoint: `PUT {{TaskManagementSystem_HostAddress}}/api/tasks/complete/{taskId}`
  - Access: Admin, Employee, Manager

- **Get Tasks for Employee**: Enables Admin, Employee, and Manager roles to retrieve tasks assigned to a specific employee.
  - Endpoint: `GET {{TaskManagementSystem_HostAddress}}/api/tasks/employee/{employeeId}`
  - Access: Admin, Employee, Manager

#### Manager Access
- **Get Tasks by Team for Manager**: Allows Managers to retrieve tasks assigned to their team.
  - Endpoint: `GET {{TaskManagementSystem_HostAddress}}/api/tasks/manager/team/{teamId}`
  - Access: Manager

#### Admin Access
- **Get Tasks Due in Week**: Allows Admins to fetch tasks that are due within the current week.
  - Endpoint: `GET {{TaskManagementSystem_HostAddress}}/api/tasks/admin/due-in-week`
  - Access: Admin

- **Get Tasks Due in Month**: Enables Admins to fetch tasks that are due within the current month.
  - Endpoint: `GET {{TaskManagementSystem_HostAddress}}/api/tasks/admin/due-in-month`
  - Access: Admin

- **Get Tasks by Team for Admin**: Allows Admins to retrieve tasks assigned to a specific team.
  - Endpoint: `GET {{TaskManagementSystem_HostAddress}}/api/tasks/admin/team/{teamId}`
  - Access: Admin

- **Get All Tasks**: Enables Admins to retrieve all tasks in the system.
  - Endpoint: `GET {{TaskManagementSystem_HostAddress}}/api/tasks/admin/all`
  - Access: Admin

## Features to Check-Out

## 1. Inheritance in UserEntity Table

We have implemented inheritance in our `UserEntity` table by creating derived entities such as `Employee`, `Admin`, and `Manager`. This approach is known as the **Table Per Hierarchy (TPH)** inheritance pattern.

### Benefits of TPH:

- **Simplicity**: TPH keeps the database schema simple by storing all entities in a single table, which can simplify queries and reduce the complexity of the database design.
- **Performance**: Since all data is stored in a single table, it can lead to faster query performance for operations that need to access multiple types of users at once.
- **Maintainability**: Changes to the base entity are automatically inherited by the derived entities, making it easier to maintain and update the data model.

## 2. Structuring APIs into One Controller (TaskController)

By structuring all task-related APIs within a single `TaskController`, we achieve several benefits:

- **Cohesion**: Keeping related functionalities together improves the logical organization of the code, making it easier for developers to understand and work with.
- **Ease of Use**: For API consumers, having a single point of access for all task-related operations can simplify the integration process and improve the overall user experience.
- **Maintainability**: Centralizing task-related logic in one controller facilitates easier updates and maintenance, as changes to task handling are localized within a single area of the application.

## 3. Providing Calculated Responses for Admin APIs

For Admin APIs, we provide calculated responses that include statistics such as `totalTasks`, `completedOnTime`, `completedLate`, `inProgress`, and `pastDue`. This is achieved through:

- **Aggregation Queries**: Utilizing SQL or ORM capabilities to perform aggregation queries that calculate these statistics based on the current state of tasks in the database.
- **Business Logic Layer**: Implementing business logic in the application layer that processes task data to compute these statistics, allowing for more complex calculations that might not be easily achievable through simple database queries.

### Benefits:

- **Insightful Reporting**: Admins receive valuable insights into task performance and team productivity, enabling informed decision-making.
- **Real-time Data Access**: By calculating these statistics on demand, admins have access to up-to-date information reflecting the current state of tasks.
- **Customization**: This approach allows for the customization of reports to meet specific administrative needs, enhancing the utility of the task management system.
