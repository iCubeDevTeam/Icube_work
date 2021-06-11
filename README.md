Postgres Database (Docker)
  - docker run --name icube -p 5432:5432 -e POSTGRES_PASSWORD=somePassword -d postgres
Test for refresh token
  - login
  Post -> https://localhost:5001/api/token/auth 
    {
	    "username": "Admin",
	    "password": "password",
      "factoryname": "icube",
      "granttype": "password"
    }
  - refresh token
  Post -> https://localhost:5001/api/token/auth 
    {
	    "refreshToken": "sAZY3tuaGm4yANziev3VabzVtc4nPqg0IGtoWQN10NavRlGrETBqnR9yPhLoOGxXNSEJYa4/TVjuRwCMrLNrHA==" , ## Use refresh token from response above 
      "granttype": "refresh_token"
    }
Test policy
 https://localhost:5001/api/Test/TagService ## Allow Administrator  
 https://localhost:5001/api/Test/InterfaceService ## Allow Administrator and Operator
 https://localhost:5001/api/Test/IntegrationService ## Allow Administrator
