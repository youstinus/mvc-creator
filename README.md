### MVC creator for .NET Core and .NET Framework applications
### How to use?
Create new project  
Copy MVCreator.exe into project's folder  
Open CMD inside project's location  
Type `MVCreator.exe` followed by arguments separated by space  
Arguments case sensitive and must represent objects  
Type `MVCreator.exe` for help  
Example  `MVCreator.exe Customer Restaurant Product Table Waiter`  

### It will create folder structure with files:  
    .
    ├── ...
    │
    ├── Base
    │   └── Interfaces
    │
    ├── Controllers
    │   └── Interfaces
    │
    ├── Repositories
    │   └── Interfaces
    │
    ├── Services
    │   └── Interfaces
    │
    └── ...
	
Base folder contains base entities, controller, repository, service.
