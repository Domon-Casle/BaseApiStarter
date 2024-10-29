# Goal of project
* Base project and domain layer setup. This is to help start new projects and move things along quickly 
 redo the section above

## Projects
* BaseApi - BE Net Api
* CoreDomain - Base logic layer to make sure consistant CRUD actions
* CoreUtilities - Common extensions, logger and so on

```mermaid
classDiagram
    namespace BaseApi {
        class EntityController {            
        }
    }

    namespace CoreDomain {
        class EntityDomain {            
        }

        class BaseDomain {
            Get()
            Create()
            Update()
            Delete()
        }

        class BaseRepository {
            Get()
            Create()
            Update()
            Delete()
        }

        class AuditDomain {
            AreThereChanges()
            AuditRead()
            AuditCreate()
            AuditChanges()
            AuditDelete()
        }
    }
    
    namespace DB {
        class PersistenceStorage {
        }
    }

    EntityController --> EntityDomain
    EntityDomain --> BaseDomain
    BaseDomain --> BaseRepository
    BaseDomain --> AuditDomain
    AuditDomain --> PersistenceStorage
    BaseRepository --> PersistenceStorage
```

## DI
* DI is setup by each project to be done on its own. 

## Unit tests
* CoreDomainUnitTests
* CoreUtilitiesUnitTests

# To contribute
* To contribute to this everything must be in a PR first. Suggestions always welcome!
