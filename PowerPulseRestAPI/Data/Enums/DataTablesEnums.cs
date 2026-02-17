namespace PowerPulseRestAPI.Data.Enums
{
    // =======================================================
    // LEAVE MANAGEMENT (urlopy / absencje)
    // =======================================================

    /// <summary>Typ urlopu</summary>
    public enum LeaveType { VACATION, SICK, UNPAID, OTHER }

    /// <summary>Status wniosku urlopowego</summary>
    public enum LeaveStatus { DRAFT, SUBMITTED, APPROVED, REJECTED, CANCELED }



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

    /// <summary>Typ zdarzenia sesji pracy</summary>
    public enum WorkSessionEventType { START, ARRIVED, BREAK_START, BREAK_END, FINISH, OTHER }



    // =======================================================
    // CUSTOMERS & RELATIONSHIPS
    // =======================================================

    /// <summary>Typ klienta</summary>
    public enum CustomerType { INDIVIDUAL, COMPANY }

    /// <summary>Status klienta</summary>
    public enum CustomerStatus { LEAD, ACTIVE, INACTIVE }

    /// <summary>Relacja klienta z projektem</summary>
    public enum ProjectCustomerRelationshipType { OWNER, CO_OWNER, MANAGER, OTHER }

    /// <summary>Rola kontaktu klienta w projekcie</summary>
    public enum ProjectCustomerContactRole { OWNER, MANAGER, CONTACT, OTHER }



    // =======================================================
    // MATERIALS & LOGISTICS
    // =======================================================

    /// <summary>Typ ruchu materiału</summary>
    public enum MaterialMovementType
    {
        RECEIPT,
        TRANSFER_TO_PROJECT,
        TRANSFER_TO_VEHICLE,
        RETURN_FROM_PROJECT,
        CONSUME_ON_PROJECT,
        ADJUSTMENT
    }



    // =======================================================
    // TOOLS & ASSETS
    // =======================================================

    /// <summary>Stan narzędzia</summary>
    public enum ToolCondition { NEW, GOOD, OK, DAMAGED, LOST }

    /// <summary>Status zasobu narzędziowego</summary>
    public enum ToolAssetStatus { IN_STOCK, ASSIGNED, IN_SERVICE, RETIRED }

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
    public enum PurchaseStatus { NEW, APPROVED, REJECTED, ORDERED, DELIVERED }



    // =======================================================
    // VEHICLES & SERVICE
    // =======================================================

    /// <summary>Status pojazdu</summary>
    public enum VehicleStatus { ACTIVE, IN_SERVICE, OUT_OF_SERVICE, SOLD }

    /// <summary>Status zatwierdzenia zlecenia serwisowego</summary>
    public enum ServiceOrderApprovalStatus { PENDING, APPROVED, REJECTED }

    /// <summary>Status zlecenia serwisowego</summary>
    public enum ServiceOrderStatus { DRAFT, PENDING_APPROVAL, APPROVED, SCHEDULED, IN_SERVICE, COMPLETED, CANCELED }

    /// <summary>Status płatności</summary>
    public enum PaidStatus { UNPAID, PAID, PARTIAL }

    /// <summary>Źródło przebiegu</summary>
    public enum MileageSourceType { MANUAL, SERVICE_ORDER, OTHER }



    // =======================================================
    // NOTIFICATIONS & ACTIVITY LOG
    // =======================================================

    /// <summary>Typ powiadomienia</summary>
    public enum NotificationType { MANAGER, SYSTEM, TASK, PURCHASE, VEHICLE, TOOL, PROJECT, OTHER }

    /// <summary>Ważność powiadomienia</summary>
    public enum NotificationSeverity { INFO, WARNING, CRITICAL }

    /// <summary>Typ encji w logu aktywności</summary>
    public enum ActivityEntityType { PROJECT, TASK, MATERIAL, TOOL, VEHICLE, SESSION, OTHER }

    /// <summary>Typ akcji w logu aktywności</summary>
    public enum ActivityActionType { CREATED, UPDATED, STATUS_CHANGED, ASSIGNED, CONSUMED, COMMENTED, ATTACHED, OTHER }



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
    public enum StockRequestStatus { DRAFT, SUBMITTED, APPROVED, REJECTED, PREPARED, ISSUED, CANCELED }

    /// <summary>Typ pozycji magazynowej</summary>
    public enum StockItemType { MATERIAL, TOOL }

    /// <summary>Status rezerwacji</summary>
    public enum ReservationStatus { ACTIVE, RELEASED, CONSUMED, CANCELED }



    // =======================================================
    // INVOICING 
    // =======================================================

    /// <summary>Status faktury</summary>
    public enum InvoiceStatus { DRAFT, ISSUED, PAID, PARTIAL, CANCELED }

    /// <summary>Typ pozycji faktury</summary>
    public enum InvoiceItemType { LABOR, MATERIAL, OTHER }

    /// <summary>Źródło pozycji faktury</summary>
    public enum InvoiceSourceType { MATERIAL_MOVEMENT, WORK_SESSION, OTHER }




    // =======================================================
    // IDENTIFIERS & HR
    // =======================================================

    /// <summary>Typ identyfikatora osoby</summary>
    public enum IdentifierType { PESEL, SSN, PASSPORT }

    /// <summary>Typ zatrudnienia</summary>
    public enum EmployeeType { UNKNOWN, FULL_TIME, PART_TIME, CONTRACTOR }

    /// <summary>Status pracownika</summary>
    public enum EmployeeStatus { ACTIVE, INACTIVE, TERMINATED }



    // =======================================================
    // GENERIC DOMAIN TYPES
    // =======================================================

    /// <summary>Typ encji systemowej</summary>
    public enum EntityType { CUSTOMER, PROJECT, PERSON, COMPANY, WAREHOUSE, DELIVERY }

    /// <summary>Typ adresu</summary>
    public enum AddressType { MAIN, BILLING, SHIPPING, PROJECT_LOCATION, HOME, OFFICE, OTHER }

    /// <summary>Typ notatki</summary>
    public enum NoteType { GENERAL, ISSUE, PROGRESS, CUSTOMER, OTHER }

    /// <summary>Typ notatki klienta</summary>
    public enum CustomerNoteType { GENERAL, CALL, EMAIL, MEETING, COMPLAINT, OTHER }

}
