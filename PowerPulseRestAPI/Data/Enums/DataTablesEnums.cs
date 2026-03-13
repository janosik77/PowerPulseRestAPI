namespace PowerPulseRestAPI.Data.Enums
{

    // =======================================================
    // PROJECT & TASK MANAGEMENT
    // =======================================================

    /// <summary>Status projektu</summary>
    public enum ProjectStatus { DRAFT, ACTIVE, ON_HOLD, COMPLETED, CANCELED }

    /// <summary>Priorytet zadania</summary>
    public enum TaskPriority { LOW, NORMAL, HIGH }

    /// <summary>Status zadania</summary>
    public enum ProjectTaskStatus { NEW, IN_PROGRESS, BLOCKED, DONE }

    /// <summary>Typ aktualizacji zadania</summary>
    public enum TaskUpdateType { STATUS_CHANGE, COMMENT, ATTACHMENT, TIME_LOG }



    // =======================================================
    // WORK SESSIONS / TIME TRACKING
    // =======================================================

    /// <summary>Status sesji pracy</summary>
    public enum WorkSessionStatus { IN_PROGRESS, PAUSED, COMPLETED }


    // =======================================================
    // CUSTOMERS & RELATIONSHIPS
    // =======================================================

    /// <summary>Typ klienta</summary>
    public enum CustomerType { INDIVIDUAL, COMPANY }

    /// <summary>Status klienta</summary>
    public enum CustomerStatus { LEAD, ACTIVE, INACTIVE }

    /// <summary>Relacja klienta z projektem</summary>
    public enum CustomerContactRole { OWNER, CO_OWNER, MANAGER, OTHER }





    // =======================================================
    // MATERIALS & LOGISTICS
    // =======================================================

    /// <summary>Typ ruchu materiału</summary>
    public enum MaterialMovementType
    {
        PURCHASE_RECEIPT = 1, // zakup / przyjęcie do magazynu
        ISSUE_TO_PROJECT = 2, // wydanie z magazynu na projekt
        RETURN_FROM_PROJECT = 3, // zwrot z projektu do magazynu
        WAREHOUSE_ADJUSTMENT = 4 // korekta magazynu
    }
    public enum MaterialTransferEndpointType
    {
        Warehouse = 1,
        Project = 2
    }
    // =======================================================
    // TOOLS & ASSETS
    // =======================================================

    /// <summary>Stan narzędzia</summary>
    public enum ToolCondition { NEW, GOOD, OK, DAMAGED, LOST }

    /// <summary>Status zasobu narzędziowego</summary>
    public enum ToolStatus { IN_STOCK, ASSIGNED, IN_SERVICE, RETIRED }

    /// <summary>Typ zgłoszenia narzędzia</summary>
    public enum ToolIssueType { DAMAGE, LOST, OTHER }

    /// <summary>Uniwersalny status obiektu</summary>
    public enum GenericStatus { NEW, IN_PROGRESS, RESOLVED, REJECTED }


    // =======================================================
    // PURCHASES / PROCUREMENT
    // =======================================================

    /// <summary>Priorytet zakupu</summary>
    public enum PurchasePriority { LOW, NORMAL, HIGH }

    /// <summary>Status zamówienia zakupu</summary>
    //public enum PurchaseStatus { NEW, APPROVED, REJECTED, ORDERED, DELIVERED }



    // =======================================================
    // VEHICLES & SERVICE
    // =======================================================

    /// <summary>Status pojazdu</summary>
    public enum VehicleStatus { ACTIVE, IN_SERVICE, OUT_OF_SERVICE, SOLD, DISPOSED }

    /// <summary>Status zatwierdzenia zlecenia serwisowego</summary>
    public enum ServiceOrderApprovalStatus { PENDING, APPROVED, REJECTED }

    /// <summary>Status zlecenia serwisowego</summary>
    public enum ServiceOrderStatus { DRAFT, PENDING_APPROVAL, APPROVED, SCHEDULED, IN_SERVICE, COMPLETED, CANCELED }

    /// <summary>Źródło przebiegu</summary>
    public enum MileageSourceType { MANUAL, SERVICE_ORDER, OTHER }



    // =======================================================
    // NOTIFICATIONS & ACTIVITY LOG
    // =======================================================

    /// <summary>Typ powiadomienia</summary>
    public enum NotificationType { MANAGER, SYSTEM, TASK, PURCHASE, VEHICLE, TOOL, PROJECT, OTHER }

    /// <summary>Ważność powiadomienia</summary>
    public enum NotificationSeverity { INFO, WARNING, CRITICAL }

    /// <summary>Typ akcji w logu aktywności</summary>
    public enum ActivityActionType { CREATED, UPDATED, STATUS_CHANGED, ASSIGNED, CONSUMED, COMMENTED, ATTACHED, OTHER }

    public enum TextTemplateChannel
    {
        UI = 1,
        PAGE = 2,
        MESSAGE = 3,
        NOTIFICATION = 4
    }


    // =======================================================
    // KNOWLEDGE BASE / CMS
    // =======================================================

    /// <summary>Typ artykułu wiedzy</summary>
    public enum KnowledgeArticleType { STANDARD, GUIDE, PROCEDURE, OTHER }

    /// <summary>Tag ważności</summary>
    public enum SeverityTag { NONE, BHP, CRITICAL }

    /// <summary>Status publikacji</summary>
    public enum PublishStatus { DRAFT, PUBLISHED, ARCHIVED }

    /// <summary>Typ załącznika</summary>
    public enum AttachmentType { PHOTO, PDF, FILE, OTHER }



    // =======================================================
    // STOCK & RESERVATIONS
    // =======================================================

    /// <summary>Typ zapotrzebowania magazynowego</summary>
    public enum StockRequestType { MATERIAL, TOOL, MIXED }

    /// <summary>Status zapotrzebowania magazynowego</summary>
    public enum StockRequestStatus {
        NEW = 1,
        PENDING = 2,
        CLOSED = 3,
        REJECTED = 4,
        CANCELLED = 5
    }

    /// <summary>Typ pozycji magazynowej</summary>
    public enum StockItemType { MATERIAL, TOOL }

    /// <summary>Status rezerwacji</summary>
    //public enum ReservationStatus { ACTIVE, RELEASED, CONSUMED, CANCELED }

    //public enum WarehouseTab{MATERIALS = 1, TOOLS = 2}

    // =======================================================
    // INVOICING 
    // =======================================================

    /// <summary>Status faktury</summary>
    public enum InvoiceStatus { DRAFT, ISSUED, PAID, PARTIAL, CANCELED }

    /// <summary>Typ pozycji faktury</summary>
    public enum InvoiceItemType { LABOR, MATERIAL }





    // =======================================================
    // IDENTIFIERS & HR
    // =======================================================

    /// <summary>Typ identyfikatora osoby</summary>
    public enum IdentifierType { PESEL, SSN, PASSPORT }

    /// <summary>Status pracownika</summary>
    public enum EmployeeStatus { ACTIVE, INACTIVE, TERMINATED }



    // =======================================================
    // GENERIC DOMAIN TYPES
    // =======================================================


    /// <summary>Typ adresu</summary>
    public enum AddressType { MAIN, BILLING, SHIPPING, PROJECT_LOCATION, HOME, OFFICE, OTHER }

    /// <summary>Typ notatki</summary>
    public enum NoteType { GENERAL, ISSUE, PROGRESS, CUSTOMER, OTHER }

    /// <summary>Typ notatki klienta</summary>
    public enum CustomerNoteType { GENERAL, CALL, EMAIL, MEETING, COMPLAINT, OTHER }

}
