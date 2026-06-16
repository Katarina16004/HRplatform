# HR Platform

HR management platform for handling job candidates and their skills

## Project Overview

The HR Platform is built with a tech stack:
- **Backend**: C# .NET 8 with clean architecture principles and SOLID principles
- **Frontend**: React with Vite + TypeScript
- **Database**: MySQL
- **Testing**: NUnit

## Features

### Core Functionality
- **Candidate Management**
  - Add new candidates with personal information
  - Update candidate details
  - Delete candidate
  - Add skill to candidate
  - Remove skill from candidate
  - Search candidates by name and/or skills
  - Get all/concrete candidate by id/name

- **Skill Management**
  - Add new skills to the system 
  - Delete skills
  - Get all/concrete skill by id/name

### Data Integrity & Constraints
- Email uniqueness 
- Skill name uniqueness
- Prevention of duplicate skill assignments to candidates
- Referential integrity between candidates and skills

### Setup:
1. Run script from Database folder to create database with seed data
2. Find HRplatform.sln in Backend/HRplatform folder and run
3. Check User Secrets (right click at HRplatform.API in VS -> Manage User Secrets)
4. Check if connection string is right for your base setup
   {
  "ConnectionStrings:Default": "Server=localhost;Port=3306;Database=job_candidates;User=root;Password=yourpassword;"
   }
5. Run project and test endpoints
6. Run tests
7. Find vite.config.ts in Frontend/HRplatform-frontend folder and check if port for backend is right
8. Do npm install in HRplatform-frontend folder
9. Do npm run dev (start backend first!)
10. Test application


## Implementation Highlights

#### Key Decisions:

- **Repository Pattern**: Created separate repository classes for each entity (SkillRepository, CandidateRepository) to abstract data access logic
- **Service Layer**: Business logic lives in services (SkillService, CandidateService), not in controllers
- **NUnit Tests**: Wrote tests for critical business logic

#### Why this approach matters:
- **Maintainability**: Clear separation makes it easy to find and fix bugs
- **Testability**: Each component can be tested independently with mocks
- **Scalability**: Adding new features doesn't require rewriting existing code
- **Professional Standard**: This architecture is used in real-world applications
