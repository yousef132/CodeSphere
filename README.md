# CodeSphere

# Contest
- **Attributes**
  - `Id`
  - `name`
  - `start_date`
  - `Duration`
- **Relationships**
  - A contest has many **Problems**
  - A contest has many **Users** (submits)
  - A contest has one **Setter**

# Problem
- **Attributes**
  - `Id`
  - `name`
  - `difficulty`
- **Relationships**
  - A problem has many **Topics**
  - A problem belongs to one **Contest**
  - A problem has many **Users** (submits)
  - A problem has many **Test Cases**
  - A problem has one **Setter**
  - A problem has one **Tutorial**

# User
- **Attributes**
  - `Id`
  - `name`
  - `email`
  - `password`
  - `role`
  - `rating` (e.g., 1500)
  - `rank` (e.g., expert)
- **Relationships**
  - A user has many **Submits**
  - A user registers for many **Contests**
  - A user creates many **Contests**
  - A user creates many **Problems**
  - A user has many **Tutorials**

# Topic
- **Attributes**
  - `Id`
  - `name`
- **Relationships**
  - A topic belongs to many **Problems**

# Test Case
- **Attributes**
  - `Id`
  - `input`
  - `output`
- **Relationships**
  - A test case belongs to one **Problem**

# Tutorial
- **Attributes**
  - `Id`
  - `content`
- **Relationships**
  - A tutorial belongs to one **Problem**
  - A tutorial belongs to one **User**




[Materialized Path](https://bojanz.wordpress.com/2014/04/25/storing-hierarchical-data-materialized-path/)

[Storing Hierarchical Data in Relational Databases](https://medium.com/@rishabhdevmanu/from-trees-to-tables-storing-hierarchical-data-in-relational-databases-a5e5e6e1bd64)


nested set vs materialized path



commentId INT NOT NULL (PRIMARY KEY)
questionId INT NOT NULL (FK to questions)
parentCommentId INT NULL (FK to comments)
commentText VARCHAR
author INT NOT NULL (FK to users)
dateSubmitted DATETIME NOT NULL
-- additional metadata as necessary; e.g.
isEdited BIT NOT NULL (Reddit displays an asterisk on edited comments)
isDeleted BIT NOT NULL
numTimesGilded INT NOT NULL (Reddit displays how many users have gilded a comment)