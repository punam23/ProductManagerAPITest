**Description**

The ProductAPImanagerTest project is a test suite designed to ensure the functionality, reliability, and correctness of the Product Management API. It uses xUnit for testing the API endpoints, validating data integrity, and simulating various scenarios for comprehensive coverage. This project supports maintaining high-quality API standards by catching issues during development.

ProductAPIManager solution - please refer it here: https://github.com/punam23/ProductAPIManager

Test Results Screenshots Attached in this repository https://github.com/punam23/ProductAPIManager/blob/master/Screenshots%20of%20endpoint%20testing%20using%20Swagger.docx

**Features**

    Unit Testing: Covers core CRUD functionalities and stock management.
    
    Validation Testing: Ensures input validation for product ID formats, stock levels, and other constraints.
    
    Error Handling Verification: Simulates invalid operations to verify proper error messages and responses.
    
    Integration Testing: Tests integration with the database using an In-Memory Database.

**Technologies Used**

    Framework: xUnit for testing
    
    Database: Entity Framework Core In-Memory Database
    
    IDE: Visual Studio

**How to Run the Tests**

    1. Clone the Repository:  git clone <repository-url>
    
    2. Open Solution in Visual Studio: 
        Open both the ProductAPIManager (https://github.com/punam23/ProductAPIManager) and 
        the ProductAPImanagerTest (https://github.com/punam23/ProductManagerAPITest) projects under same solution.
    
    3. Set Up Database: Ensure the In-Memory Database is configured correctly in the test project.
    
    4. Run Tests:
    
        Open the Test Explorer in Visual Studio.
        
        Click Run All to execute all unit tests.
        
    5. View Results:
        The test results will display in the Test Explorer, indicating which tests passed or failed.
