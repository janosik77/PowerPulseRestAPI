namespace PowerPulseRestAPI.Data.Enums
{

    // =======================================================
    // PROJECT & TASK MANAGEMENT
    // =======================================================

    /// <summary>Status projektu</summary>
    public enum ProjectStatus { DRAFT, ACTIVE, ON_HOLD, COMPLETED, CANCELED }

    /// <summary>Status zadania</summary>
    public enum ProjectTaskStatus { NEW, IN_PROGRESS, BLOCKED, DONE }


    // =======================================================
    // CUSTOMERS & RELATIONSHIPS
    // =======================================================

    /// <summary>Status klienta</summary>
    public enum CustomerStatus {NOT_ASSIGNET = 0, LEAD = 1 , ACTIVE = 2, INACTIVE= 3 }

    // =======================================================
    // MATERIALS & LOGISTICS
    // =======================================================

    /// <summary>Typ ruchu materiału</summary>
    public enum MaterialMovementType
    {
        PURCHASE_RECEIPT = 1, // zakup / przyjęcie do magazynu
        ISSUE_TO_PROJECT = 2, // wydanie z magazynu na projekt
        RETURN_FROM_PROJECT = 3, // zwrot z projektu do magazynu
        WAREHOUSE_ADJUSTMENT_INCREASE = 4,
        WAREHOUSE_ADJUSTMENT_DECREASE = 5,
        PROJECT_CONSUME = 6,
        PROJECT_CONSUME_ADJUSTMENT_DECREASE = 7,
    }
    // =======================================================
    // TOOLS & ASSETS
    // =======================================================

    /// <summary>Stan narzędzia</summary>
    public enum ToolCondition { NEW, GOOD, OK, DAMAGED, LOST }

    /// <summary>Status zasobu narzędziowego</summary>
    public enum ToolStatus { IN_STOCK, ASSIGNED, IN_SERVICE, RETIRED }

    /// <summary>Uniwersalny status obiektu</summary>
    public enum GenericStatus {OPEN, IN_PROGRESS, RESOLVED}


    // =======================================================
    // PURCHASES / PROCUREMENT
    // =======================================================

    /// <summary>Priorytet zakupu</summary>
    public enum PurchasePriority { LOW, NORMAL, HIGH }

    // =======================================================
    // VEHICLES & SERVICE
    // =======================================================

    /// <summary>Status pojazdu</summary>
    public enum VehicleStatus { AVAILABLE, ASSIGNED, IN_SERVICE, OUT_OF_SERVICE, SOLD, DISPOSED }

    // =======================================================
    // NOTIFICATIONS & ACTIVITY LOG
    // =======================================================

    /// <summary>Typ powiadomienia</summary>
    public enum NotificationType { MANAGER, SYSTEM, TASK, PURCHASE, VEHICLE, TOOL, PROJECT, OTHER }

    /// <summary>Ważność powiadomienia</summary>
    public enum NotificationSeverity { INFO, WARNING, CRITICAL }

    public enum TextTemplateChannel
    {
        UI = 1,
        PAGE = 2,
    }


    // =======================================================
    // KNOWLEDGE BASE / CMS
    // =======================================================

    /// <summary>Status publikacji</summary>
    public enum PublishStatus { DRAFT, PUBLISHED, ARCHIVED }

    /// <summary>Typ załącznika</summary>
    public enum AttachmentType { PHOTO, PDF, FILE, OTHER }


    // =======================================================
    // STOCK & RESERVATIONS
    // =======================================================

    /// <summary>Typ pozycji magazynowej</summary>
    public enum StockItemType { MATERIAL, TOOL }

    // =======================================================
    // WORK SESSIONS & TIME TRACKING
    // =======================================================

    /// <summary>Status sesji pracy</summary>
    public enum WorkSessionStatus { ACTIVE, CLOSED }

    // =======================================================
    // INVOICING 
    // =======================================================

    /// <summary>Status faktury</summary>
    public enum InvoiceStatus {DRAFT, ISSUED, PAID, CANCELED }
    public enum InvoiceFormItemType
    {
        LABOR = 1,
        MATERIAL = 2
    }

    // =======================================================
    // IDENTIFIERS & HR
    // =======================================================

    /// <summary>Status pracownika</summary>
    public enum EmployeeStatus { ACTIVE, INACTIVE, TERMINATED }

    // =======================================================
    // GENERIC DOMAIN TYPES
    // =======================================================

    /// <summary>Typ adresu</summary>
    public enum AddressType { 
        MAIN = 0, 
        BILLING = 1, 
        PROJECT_LOCATION = 2, 
        HOME =3 , OFFICE = 4, 
        OTHER = 5
    }

    /// <summary>Typ notatki</summary>
    public enum NoteType { GENERAL, ISSUE, PROGRESS, CUSTOMER, OTHER }


}
