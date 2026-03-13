using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerPulseRestAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    postal_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    city = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    building_number = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    apartment_number = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    full_text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                    table.CheckConstraint("ck_address_lat_range", "latitude IS NULL OR (latitude >= -90 AND latitude <= 90)");
                    table.CheckConstraint("ck_address_lon_range", "longitude IS NULL OR (longitude >= -180 AND longitude <= 180)");
                });

            migrationBuilder.CreateTable(
                name: "calculators",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calculators", x => x.id);
                    table.CheckConstraint("ck_calculator_code_not_empty", "code <> ''");
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    tax_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_categories", x => x.id);
                    table.CheckConstraint("ck_knowledge_category_not_self_parent", "parent_id IS NULL OR parent_id <> id");
                    table.ForeignKey(
                        name: "FK_knowledge_categories_knowledge_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "knowledge_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "material_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_categories", x => x.id);
                    table.CheckConstraint("ck_material_category_not_self_parent", "parent_id IS NULL OR parent_id <> id");
                    table.ForeignKey(
                        name: "FK_material_categories_material_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "material_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                    table.CheckConstraint("ck_role_name_not_empty", "name <> ''");
                });

            migrationBuilder.CreateTable(
                name: "storage_locations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_storage_locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tool_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_categories", x => x.id);
                    table.CheckConstraint("ck_tool_category_not_self_parent", "parent_id IS NULL OR parent_id <> id");
                    table.ForeignKey(
                        name: "FK_tool_categories_tool_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "tool_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    plate_number = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    vin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    make = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    year = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    current_mileage = table.Column<int>(type: "int", nullable: true),
                    last_service_at = table.Column<DateOnly>(type: "date", nullable: true),
                    last_service_mileage = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.id);
                    table.CheckConstraint("ck_vehicle_last_service_mileage_nonneg", "last_service_mileage IS NULL OR last_service_mileage >= 0");
                    table.CheckConstraint("ck_vehicle_mileage_nonneg", "current_mileage IS NULL OR current_mileage >= 0");
                    table.CheckConstraint("ck_vehicle_year_range", "year IS NULL OR (year >= 1900 AND year <= 2100)");
                });

            migrationBuilder.CreateTable(
                name: "entity_addresses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    address_id = table.Column<long>(type: "bigint", nullable: false),
                    entity_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    entity_id = table.Column<long>(type: "bigint", nullable: false),
                    address_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    is_primary = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_addresses", x => x.id);
                    table.CheckConstraint("ck_entity_address_entity_id_positive", "entity_id > 0");
                    table.ForeignKey(
                        name: "FK_entity_addresses_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_contacts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    role_title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    is_primary = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_contacts", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_contacts_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sku = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    default_unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.id);
                    table.CheckConstraint("ck_material_default_unit_not_empty", "default_unit <> ''");
                    table.ForeignKey(
                        name: "FK_materials_material_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "material_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    last_password_update = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_login_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RoleId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.CheckConstraint("ck_user_email_not_empty", "email <> ''");
                    table.CheckConstraint("ck_user_login_not_empty", "login <> ''");
                    table.CheckConstraint("ck_user_password_hash_not_empty", "password_hash <> ''");
                    table.ForeignKey(
                        name: "FK_users_roles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "roles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tools",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sku = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    model = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tools", x => x.id);
                    table.ForeignKey(
                        name: "FK_tools_tool_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "tool_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "material_images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_id = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    is_primary = table.Column<bool>(type: "bit", nullable: false),
                    alt_text = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_images", x => x.id);
                    table.CheckConstraint("ck_material_image_sort_nonneg", "sort_order >= 0");
                    table.ForeignKey(
                        name: "FK_material_images_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "material_price_lists",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_id = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    valid_from = table.Column<DateOnly>(type: "date", nullable: false),
                    valid_to = table.Column<DateOnly>(type: "date", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_price_lists", x => x.id);
                    table.CheckConstraint("ck_material_price_list_price_nonneg", "price >= 0");
                    table.CheckConstraint("ck_material_price_list_valid_range", "valid_to IS NULL OR valid_to >= valid_from");
                    table.ForeignKey(
                        name: "FK_material_price_lists_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "material_stock",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_id = table.Column<long>(type: "bigint", nullable: false),
                    storage_location_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_stock", x => x.id);
                    table.CheckConstraint("ck_material_stock_qty_nonneg", "quantity >= 0");
                    table.ForeignKey(
                        name: "FK_material_stock_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_stock_storage_locations_storage_location_id",
                        column: x => x.storage_location_id,
                        principalTable: "storage_locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "material_vehicle_balance",
                columns: table => new
                {
                    material_id = table.Column<long>(type: "bigint", nullable: false),
                    vehicle_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_vehicle_balance", x => new { x.material_id, x.vehicle_id });
                    table.CheckConstraint("ck_material_vehicle_balance_qty_nonneg", "quantity >= 0");
                    table.ForeignKey(
                        name: "FK_material_vehicle_balance_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_vehicle_balance_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "activity_log",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    entity_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    entity_id = table.Column<long>(type: "bigint", nullable: false),
                    action_type = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_log", x => x.id);
                    table.CheckConstraint("ck_activity_log_entity_id_positive", "entity_id > 0");
                    table.ForeignKey(
                        name: "FK_activity_log_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "customer_notes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    note_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_notes", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_notes_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_customer_notes_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_articles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: false),
                    article_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    severity_tag = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    published_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    updated_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_articles", x => x.id);
                    table.CheckConstraint("ck_knowledge_article_publish_consistency", "(status <> 'PUBLISHED' AND published_at IS NULL) OR (status = 'PUBLISHED' AND published_at IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_knowledge_articles_knowledge_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "knowledge_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_knowledge_articles_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_knowledge_articles_users_updated_by_user_id",
                        column: x => x.updated_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    avatar_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    priority = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    requested_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    requested_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    approval_status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    approved_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    approved_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    approval_note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    service_provider_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    service_provider_phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    appointment_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    appointment_note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    status = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    sent_to_service_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    returned_from_service_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    mileage_at_service = table.Column<int>(type: "int", nullable: true),
                    final_description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    total_cost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    invoice_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    paid_status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    completed_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    completed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_orders", x => x.id);
                    table.CheckConstraint("ck_service_order_approval_consistency", "(approved_at IS NULL AND approved_by_user_id IS NULL) OR (approved_at IS NOT NULL AND approved_by_user_id IS NOT NULL)");
                    table.CheckConstraint("ck_service_order_completion_consistency", "(completed_at IS NULL AND completed_by_user_id IS NULL) OR (completed_at IS NOT NULL AND completed_by_user_id IS NOT NULL)");
                    table.CheckConstraint("ck_service_order_mileage_nonneg", "mileage_at_service IS NULL OR mileage_at_service >= 0");
                    table.CheckConstraint("ck_service_order_service_dates", "returned_from_service_at IS NULL OR sent_to_service_at IS NULL OR returned_from_service_at >= sent_to_service_at");
                    table.CheckConstraint("ck_service_order_total_cost_nonneg", "total_cost IS NULL OR total_cost >= 0");
                    table.ForeignKey(
                        name: "FK_service_orders_users_approved_by_user_id",
                        column: x => x.approved_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_service_orders_users_completed_by_user_id",
                        column: x => x.completed_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_service_orders_users_requested_by_user_id",
                        column: x => x.requested_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_service_orders_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_issues",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<long>(type: "bigint", nullable: false),
                    reported_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_issues", x => x.id);
                    table.ForeignKey(
                        name: "FK_vehicle_issues_users_reported_by_user_id",
                        column: x => x.reported_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicle_issues_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_mileage_records",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<long>(type: "bigint", nullable: false),
                    mileage = table.Column<int>(type: "int", nullable: false),
                    recorded_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    source_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_mileage_records", x => x.id);
                    table.CheckConstraint("ck_vehicle_mileage_record_nonneg", "mileage >= 0");
                    table.ForeignKey(
                        name: "FK_vehicle_mileage_records_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicle_mileage_records_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tool_assets",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tool_id = table.Column<long>(type: "bigint", nullable: false),
                    serial_number = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    asset_tag = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    condition = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_assets", x => x.id);
                    table.ForeignKey(
                        name: "FK_tool_assets_tools_tool_id",
                        column: x => x.tool_id,
                        principalTable: "tools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tool_images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tool_id = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    is_primary = table.Column<bool>(type: "bit", nullable: false),
                    alt_text = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_images", x => x.id);
                    table.CheckConstraint("ck_tool_image_sort_nonneg", "sort_order >= 0");
                    table.ForeignKey(
                        name: "FK_tool_images_tools_tool_id",
                        column: x => x.tool_id,
                        principalTable: "tools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_article_attachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    article_id = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    attachment_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_article_attachments", x => x.id);
                    table.CheckConstraint("ck_knowledge_attachment_sort_nonneg", "sort_order >= 0");
                    table.ForeignKey(
                        name: "FK_knowledge_article_attachments_knowledge_articles_article_id",
                        column: x => x.article_id,
                        principalTable: "knowledge_articles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_article_favorites",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    article_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_article_favorites", x => x.id);
                    table.ForeignKey(
                        name: "FK_knowledge_article_favorites_knowledge_articles_article_id",
                        column: x => x.article_id,
                        principalTable: "knowledge_articles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_knowledge_article_favorites_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "knowledge_article_reads",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    article_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    read_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_knowledge_article_reads", x => x.id);
                    table.ForeignKey(
                        name: "FK_knowledge_article_reads_knowledge_articles_article_id",
                        column: x => x.article_id,
                        principalTable: "knowledge_articles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_knowledge_article_reads_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    person_id = table.Column<long>(type: "bigint", nullable: false),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: false),
                    terminated_at = table.Column<DateOnly>(type: "date", nullable: true),
                    department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    employee_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    remaining_vacation_days = table.Column<int>(type: "int", nullable: false),
                    vacation_days_per_year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.id);
                    table.CheckConstraint("ck_employee_terminated_after_hire", "terminated_at IS NULL OR terminated_at >= hire_date");
                    table.CheckConstraint("ck_employee_vacation_nonneg", "remaining_vacation_days >= 0 AND vacation_days_per_year >= 0");
                    table.ForeignKey(
                        name: "FK_employee_person_person_id",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "person_identifiers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    person_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    value_encrypted = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    last4 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    country = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_identifiers", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_identifiers_person_person_id",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_order_history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    service_order_id = table.Column<long>(type: "bigint", nullable: false),
                    old_status = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    new_status = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    changed_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    changed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_order_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_order_history_service_orders_service_order_id",
                        column: x => x.service_order_id,
                        principalTable: "service_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_service_order_history_users_changed_by_user_id",
                        column: x => x.changed_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_issue_attachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_issue_id = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_issue_attachments", x => x.id);
                    table.ForeignKey(
                        name: "FK_vehicle_issue_attachments_vehicle_issues_vehicle_issue_id",
                        column: x => x.vehicle_issue_id,
                        principalTable: "vehicle_issues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tool_asset_stock",
                columns: table => new
                {
                    tool_asset_id = table.Column<long>(type: "bigint", nullable: false),
                    storage_location_id = table.Column<long>(type: "bigint", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_asset_stock", x => x.tool_asset_id);
                    table.ForeignKey(
                        name: "FK_tool_asset_stock_storage_locations_storage_location_id",
                        column: x => x.storage_location_id,
                        principalTable: "storage_locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tool_asset_stock_tool_assets_tool_asset_id",
                        column: x => x.tool_asset_id,
                        principalTable: "tool_assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tool_issues",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tool_asset_id = table.Column<long>(type: "bigint", nullable: false),
                    reported_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    issue_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_issues", x => x.id);
                    table.ForeignKey(
                        name: "FK_tool_issues_tool_assets_tool_asset_id",
                        column: x => x.tool_asset_id,
                        principalTable: "tool_assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tool_issues_users_reported_by_user_id",
                        column: x => x.reported_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "employee_bank_accounts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    account_encrypted = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    account_last4 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    country = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    is_primary = table.Column<bool>(type: "bit", nullable: false),
                    valid_from = table.Column<DateOnly>(type: "date", nullable: true),
                    valid_to = table.Column<DateOnly>(type: "date", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_bank_accounts", x => x.id);
                    table.CheckConstraint("ck_employee_bank_account_dates", "valid_to IS NULL OR valid_to >= valid_from");
                    table.ForeignKey(
                        name: "FK_employee_bank_accounts_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_compensations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    hourly_wage_encrypted = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    extra_pension_encrypted = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    valid_from = table.Column<DateOnly>(type: "date", nullable: false),
                    valid_to = table.Column<DateOnly>(type: "date", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_compensations", x => x.id);
                    table.CheckConstraint("ck_employee_compensation_dates", "valid_to IS NULL OR valid_to >= valid_from");
                    table.ForeignKey(
                        name: "FK_employee_compensations_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "leave_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    leave_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    requested_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    approved_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    approved_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leave_requests", x => x.id);
                    table.CheckConstraint("ck_leave_dates", "end_date >= start_date");
                    table.ForeignKey(
                        name: "FK_leave_requests_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_leave_requests_users_approved_by_user_id",
                        column: x => x.approved_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_leave_requests_users_requested_by_user_id",
                        column: x => x.requested_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    responsible_employee_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.id);
                    table.CheckConstraint("ck_project_dates", "end_date IS NULL OR start_date IS NULL OR end_date >= start_date");
                    table.ForeignKey(
                        name: "FK_projects_employee_responsible_employee_id",
                        column: x => x.responsible_employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_projects_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_assignments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_id = table.Column<long>(type: "bigint", nullable: false),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    assigned_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    returned_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_assignments", x => x.id);
                    table.CheckConstraint("ck_vehicle_assignment_return_after_assign", "returned_at IS NULL OR returned_at >= assigned_at");
                    table.ForeignKey(
                        name: "FK_vehicle_assignments_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicle_assignments_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tool_issue_attachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tool_issue_id = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_issue_attachments", x => x.id);
                    table.ForeignKey(
                        name: "FK_tool_issue_attachments_tool_issues_tool_issue_id",
                        column: x => x.tool_issue_id,
                        principalTable: "tool_issues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "billing_rates",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    hourly_rate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    valid_from = table.Column<DateOnly>(type: "date", nullable: false),
                    valid_to = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billing_rates", x => x.id);
                    table.CheckConstraint("ck_billing_rate_currency_not_empty", "currency <> ''");
                    table.CheckConstraint("ck_billing_rate_hourly_nonneg", "hourly_rate >= 0");
                    table.CheckConstraint("ck_billing_rate_name_not_empty", "name <> ''");
                    table.CheckConstraint("ck_billing_rate_valid_range", "valid_to IS NULL OR valid_to >= valid_from");
                    table.ForeignKey(
                        name: "FK_billing_rates_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_number = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    project_id = table.Column<long>(type: "bigint", nullable: true),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    issue_date = table.Column<DateOnly>(type: "date", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    subtotal_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    tax_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    issued_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    issued_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    customer_name_snapshot = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    customer_tax_id_snapshot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    billing_address_snapshot = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.id);
                    table.CheckConstraint("ck_invoice_amounts_nonneg", "subtotal_amount >= 0 AND tax_amount >= 0 AND total_amount >= 0");
                    table.CheckConstraint("ck_invoice_due_after_issue", "due_date >= issue_date");
                    table.CheckConstraint("ck_invoice_issue_consistency", "(issued_at IS NULL AND issued_by_user_id IS NULL) OR (issued_at IS NOT NULL AND issued_by_user_id IS NOT NULL)");
                    table.CheckConstraint("ck_invoice_total_match", "total_amount = subtotal_amount + tax_amount");
                    table.ForeignKey(
                        name: "FK_invoices_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invoices_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invoices_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invoices_users_issued_by_user_id",
                        column: x => x.issued_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "material_movements",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_id = table.Column<long>(type: "bigint", nullable: false),
                    transaction_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    from_storage_location_id = table.Column<long>(type: "bigint", nullable: true),
                    to_storage_location_id = table.Column<long>(type: "bigint", nullable: true),
                    to_project_id = table.Column<long>(type: "bigint", nullable: true),
                    to_vehicle_id = table.Column<long>(type: "bigint", nullable: true),
                    to_employee_id = table.Column<long>(type: "bigint", nullable: true),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    occurred_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_movements", x => x.id);
                    table.CheckConstraint("ck_material_movement_qty_positive", "quantity > 0");
                    table.CheckConstraint("ck_material_movement_single_target", "( (CASE WHEN to_storage_location_id IS NULL THEN 0 ELSE 1 END) +  (CASE WHEN to_project_id IS NULL THEN 0 ELSE 1 END) +  (CASE WHEN to_vehicle_id IS NULL THEN 0 ELSE 1 END) +  (CASE WHEN to_employee_id IS NULL THEN 0 ELSE 1 END) ) = 1");
                    table.CheckConstraint("ck_material_movement_unit_not_empty", "unit <> ''");
                    table.ForeignKey(
                        name: "FK_material_movements_employee_to_employee_id",
                        column: x => x.to_employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_movements_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_movements_projects_to_project_id",
                        column: x => x.to_project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_movements_storage_locations_from_storage_location_id",
                        column: x => x.from_storage_location_id,
                        principalTable: "storage_locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_movements_storage_locations_to_storage_location_id",
                        column: x => x.to_storage_location_id,
                        principalTable: "storage_locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_movements_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_movements_vehicles_to_vehicle_id",
                        column: x => x.to_vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "material_project_balance",
                columns: table => new
                {
                    material_id = table.Column<long>(type: "bigint", nullable: false),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_project_balance", x => new { x.material_id, x.project_id });
                    table.CheckConstraint("ck_material_project_balance_qty_nonneg", "quantity >= 0");
                    table.ForeignKey(
                        name: "FK_material_project_balance_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_project_balance_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_access",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    valid_from = table.Column<DateOnly>(type: "date", nullable: true),
                    valid_to = table.Column<DateOnly>(type: "date", nullable: true),
                    is_enabled = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_access", x => x.id);
                    table.CheckConstraint("ck_project_access_dates", "valid_to IS NULL OR valid_from IS NULL OR valid_to >= valid_from");
                    table.ForeignKey(
                        name: "FK_project_access_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_access_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_customer_contacts",
                columns: table => new
                {
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    customer_contact_id = table.Column<long>(type: "bigint", nullable: false),
                    contact_role = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_customer_contacts", x => new { x.project_id, x.customer_contact_id, x.contact_role });
                    table.ForeignKey(
                        name: "FK_project_customer_contacts_customer_contacts_customer_contact_id",
                        column: x => x.customer_contact_id,
                        principalTable: "customer_contacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_customer_contacts_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_customers",
                columns: table => new
                {
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    customer_id = table.Column<long>(type: "bigint", nullable: false),
                    relationship_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    is_primary_owner = table.Column<bool>(type: "bit", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_customers", x => new { x.project_id, x.customer_id, x.relationship_type });
                    table.ForeignKey(
                        name: "FK_project_customers_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_customers_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_tasks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    priority = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    due_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    estimated_minutes = table.Column<int>(type: "int", nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    assigned_to_employee_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_tasks", x => x.id);
                    table.CheckConstraint("ck_project_task_estimate_positive", "estimated_minutes IS NULL OR estimated_minutes >= 0");
                    table.ForeignKey(
                        name: "FK_project_tasks_employee_assigned_to_employee_id",
                        column: x => x.assigned_to_employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_project_tasks_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_tasks_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "purchase_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    requested_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    priority = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    approved_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    approved_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_requests", x => x.id);
                    table.CheckConstraint("ck_purchase_request_approval_consistency", "(approved_at IS NULL AND approved_by_user_id IS NULL) OR (approved_at IS NOT NULL AND approved_by_user_id IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_purchase_requests_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_purchase_requests_users_approved_by_user_id",
                        column: x => x.approved_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_purchase_requests_users_requested_by_user_id",
                        column: x => x.requested_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    request_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    priority = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    project_id = table.Column<long>(type: "bigint", nullable: true),
                    vehicle_id = table.Column<long>(type: "bigint", nullable: true),
                    employee_id = table.Column<long>(type: "bigint", nullable: true),
                    requested_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    requested_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    approved_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    approved_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    approval_note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_requests", x => x.id);
                    table.CheckConstraint("ck_stock_request_approval_consistency", "(approved_at IS NULL AND approved_by_user_id IS NULL) OR (approved_at IS NOT NULL AND approved_by_user_id IS NOT NULL)");
                    table.CheckConstraint("ck_stock_request_single_scope", "( (CASE WHEN project_id IS NULL THEN 0 ELSE 1 END) +  (CASE WHEN vehicle_id IS NULL THEN 0 ELSE 1 END) +  (CASE WHEN employee_id IS NULL THEN 0 ELSE 1 END) ) = 1");
                    table.ForeignKey(
                        name: "FK_stock_requests_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_requests_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_requests_users_approved_by_user_id",
                        column: x => x.approved_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_requests_users_requested_by_user_id",
                        column: x => x.requested_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_requests_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tool_assignments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tool_asset_id = table.Column<long>(type: "bigint", nullable: false),
                    to_storage_location_id = table.Column<long>(type: "bigint", nullable: true),
                    to_vehicle_id = table.Column<long>(type: "bigint", nullable: true),
                    to_project_id = table.Column<long>(type: "bigint", nullable: true),
                    to_employee_id = table.Column<long>(type: "bigint", nullable: true),
                    assigned_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    returned_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tool_assignments", x => x.id);
                    table.CheckConstraint("ck_tool_assignment_return_after_assign", "returned_at IS NULL OR returned_at >= assigned_at");
                    table.CheckConstraint("ck_tool_assignment_single_target", "( (CASE WHEN to_storage_location_id IS NULL THEN 0 ELSE 1 END) +  (CASE WHEN to_vehicle_id IS NULL THEN 0 ELSE 1 END) +  (CASE WHEN to_project_id IS NULL THEN 0 ELSE 1 END) +  (CASE WHEN to_employee_id IS NULL THEN 0 ELSE 1 END) ) = 1");
                    table.ForeignKey(
                        name: "FK_tool_assignments_employee_to_employee_id",
                        column: x => x.to_employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tool_assignments_projects_to_project_id",
                        column: x => x.to_project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tool_assignments_storage_locations_to_storage_location_id",
                        column: x => x.to_storage_location_id,
                        principalTable: "storage_locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tool_assignments_tool_assets_tool_asset_id",
                        column: x => x.tool_asset_id,
                        principalTable: "tool_assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tool_assignments_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tool_assignments_vehicles_to_vehicle_id",
                        column: x => x.to_vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "work_sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ended_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    started_by_user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_sessions", x => x.id);
                    table.CheckConstraint("ck_work_session_end_after_start", "ended_at IS NULL OR ended_at >= started_at");
                    table.ForeignKey(
                        name: "FK_work_sessions_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_work_sessions_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_work_sessions_users_started_by_user_id",
                        column: x => x.started_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "invoice_history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_id = table.Column<long>(type: "bigint", nullable: false),
                    old_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    new_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    changed_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    changed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_invoice_history_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_invoice_history_users_changed_by_user_id",
                        column: x => x.changed_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "invoice_items",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_id = table.Column<long>(type: "bigint", nullable: false),
                    item_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    tax_rate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    line_subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    line_tax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    line_total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_items", x => x.id);
                    table.CheckConstraint("ck_invoice_item_amounts_nonneg", "unit_price >= 0 AND line_subtotal >= 0 AND line_tax >= 0 AND line_total >= 0");
                    table.CheckConstraint("ck_invoice_item_qty_positive", "quantity > 0");
                    table.CheckConstraint("ck_invoice_item_sort_nonneg", "sort_order >= 0");
                    table.CheckConstraint("ck_invoice_item_total_match", "line_total = line_subtotal + line_tax");
                    table.ForeignKey(
                        name: "FK_invoice_items_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_attachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    task_id = table.Column<long>(type: "bigint", nullable: false),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_attachments", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_attachments_project_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "project_tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_updates",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    task_id = table.Column<long>(type: "bigint", nullable: false),
                    update_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_updates", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_updates_project_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "project_tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_updates_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    severity = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    is_read = table.Column<bool>(type: "bit", nullable: false),
                    read_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    related_project_id = table.Column<long>(type: "bigint", nullable: true),
                    related_task_id = table.Column<long>(type: "bigint", nullable: true),
                    related_purchase_request_id = table.Column<long>(type: "bigint", nullable: true),
                    related_vehicle_id = table.Column<long>(type: "bigint", nullable: true),
                    related_tool_asset_id = table.Column<long>(type: "bigint", nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.CheckConstraint("ck_notification_read_consistency", "(is_read = 0 AND read_at IS NULL) OR (is_read = 1 AND read_at IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_notifications_project_tasks_related_task_id",
                        column: x => x.related_task_id,
                        principalTable: "project_tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notifications_projects_related_project_id",
                        column: x => x.related_project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notifications_purchase_requests_related_purchase_request_id",
                        column: x => x.related_purchase_request_id,
                        principalTable: "purchase_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notifications_tool_assets_related_tool_asset_id",
                        column: x => x.related_tool_asset_id,
                        principalTable: "tool_assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notifications_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notifications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notifications_vehicles_related_vehicle_id",
                        column: x => x.related_vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "purchase_request_items",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    purchase_request_id = table.Column<long>(type: "bigint", nullable: false),
                    item_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    material_id = table.Column<long>(type: "bigint", nullable: true),
                    tool_id = table.Column<long>(type: "bigint", nullable: true),
                    item_name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_request_items", x => x.id);
                    table.CheckConstraint("ck_purchase_request_item_type_fields", "( (item_type = 'MATERIAL' AND material_id IS NOT NULL AND tool_id IS NULL AND quantity IS NOT NULL AND quantity > 0 AND unit IS NOT NULL AND unit <> '') OR  (item_type = 'TOOL' AND tool_id IS NOT NULL AND material_id IS NULL AND quantity IS NULL AND unit IS NULL))");
                    table.ForeignKey(
                        name: "FK_purchase_request_items_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_purchase_request_items_purchase_requests_purchase_request_id",
                        column: x => x.purchase_request_id,
                        principalTable: "purchase_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_purchase_request_items_tools_tool_id",
                        column: x => x.tool_id,
                        principalTable: "tools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_request_fulfillments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stock_request_id = table.Column<long>(type: "bigint", nullable: false),
                    issued_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    issued_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_request_fulfillments", x => x.id);
                    table.ForeignKey(
                        name: "FK_stock_request_fulfillments_stock_requests_stock_request_id",
                        column: x => x.stock_request_id,
                        principalTable: "stock_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stock_request_fulfillments_users_issued_by_user_id",
                        column: x => x.issued_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_request_history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stock_request_id = table.Column<long>(type: "bigint", nullable: false),
                    old_status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    new_status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    changed_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    changed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_request_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_stock_request_history_stock_requests_stock_request_id",
                        column: x => x.stock_request_id,
                        principalTable: "stock_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stock_request_history_users_changed_by_user_id",
                        column: x => x.changed_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_request_items",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stock_request_id = table.Column<long>(type: "bigint", nullable: false),
                    item_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    material_id = table.Column<long>(type: "bigint", nullable: true),
                    requested_quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    tool_id = table.Column<long>(type: "bigint", nullable: true),
                    requested_count = table.Column<int>(type: "int", nullable: true),
                    note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_request_items", x => x.id);
                    table.CheckConstraint("ck_stock_request_item_type_fields", "( (item_type = 'MATERIAL' AND material_id IS NOT NULL AND tool_id IS NULL AND requested_quantity IS NOT NULL AND requested_quantity > 0 AND unit IS NOT NULL AND unit <> '' AND requested_count IS NULL) OR  (item_type = 'TOOL' AND tool_id IS NOT NULL AND material_id IS NULL AND requested_count IS NOT NULL AND requested_count > 0 AND requested_quantity IS NULL AND unit IS NULL))");
                    table.ForeignKey(
                        name: "FK_stock_request_items_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_request_items_stock_requests_stock_request_id",
                        column: x => x.stock_request_id,
                        principalTable: "stock_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stock_request_items_tools_tool_id",
                        column: x => x.tool_id,
                        principalTable: "tools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_attachments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    work_session_id = table.Column<long>(type: "bigint", nullable: true),
                    url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    attachment_type = table.Column<int>(type: "int", nullable: false),
                    created_by_user_idW = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_attachments", x => x.id);
                    table.ForeignKey(
                        name: "FK_project_attachments_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_attachments_users_created_by_user_idW",
                        column: x => x.created_by_user_idW,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_attachments_work_sessions_work_session_id",
                        column: x => x.work_session_id,
                        principalTable: "work_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "project_notes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    work_session_id = table.Column<long>(type: "bigint", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    note_type = table.Column<int>(type: "int", nullable: false),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_notes", x => x.id);
                    table.CheckConstraint("ck_project_notes_updated_at", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "FK_project_notes_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_notes_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_project_notes_work_sessions_work_session_id",
                        column: x => x.work_session_id,
                        principalTable: "work_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "work_session_events",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    work_session_id = table.Column<long>(type: "bigint", nullable: false),
                    event_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    event_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_session_events", x => x.id);
                    table.CheckConstraint("ck_work_session_event_not_future", "event_at <= SYSDATETIMEOFFSET()");
                    table.ForeignKey(
                        name: "FK_work_session_events_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_work_session_events_work_sessions_work_session_id",
                        column: x => x.work_session_id,
                        principalTable: "work_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_item_sources",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invoice_item_id = table.Column<long>(type: "bigint", nullable: false),
                    source_type = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    source_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_item_sources", x => x.id);
                    table.CheckConstraint("ck_invoice_item_source_qty_positive", "quantity > 0");
                    table.CheckConstraint("ck_invoice_item_source_source_id_positive", "source_id > 0");
                    table.ForeignKey(
                        name: "FK_invoice_item_sources_invoice_items_invoice_item_id",
                        column: x => x.invoice_item_id,
                        principalTable: "invoice_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stock_request_fulfillment_items",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fulfillment_id = table.Column<long>(type: "bigint", nullable: false),
                    item_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    material_id = table.Column<long>(type: "bigint", nullable: true),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    from_storage_location_id = table.Column<long>(type: "bigint", nullable: true),
                    tool_asset_id = table.Column<long>(type: "bigint", nullable: true),
                    note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_request_fulfillment_items", x => x.id);
                    table.CheckConstraint("ck_stock_fulfillment_item_type_fields", "( (item_type = 'MATERIAL' AND material_id IS NOT NULL AND quantity IS NOT NULL AND quantity > 0 AND unit IS NOT NULL AND unit <> '' AND from_storage_location_id IS NOT NULL AND tool_asset_id IS NULL) OR  (item_type = 'TOOL' AND tool_asset_id IS NOT NULL AND material_id IS NULL AND quantity IS NULL AND unit IS NULL))");
                    table.ForeignKey(
                        name: "FK_stock_request_fulfillment_items_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_request_fulfillment_items_stock_request_fulfillments_fulfillment_id",
                        column: x => x.fulfillment_id,
                        principalTable: "stock_request_fulfillments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stock_request_fulfillment_items_storage_locations_from_storage_location_id",
                        column: x => x.from_storage_location_id,
                        principalTable: "storage_locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_request_fulfillment_items_tool_assets_tool_asset_id",
                        column: x => x.tool_asset_id,
                        principalTable: "tool_assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_request_reservations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stock_request_item_id = table.Column<long>(type: "bigint", nullable: false),
                    item_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    material_id = table.Column<long>(type: "bigint", nullable: true),
                    reserved_quantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    storage_location_id = table.Column<long>(type: "bigint", nullable: true),
                    tool_asset_id = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    reserved_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    reserved_by_user_id = table.Column<long>(type: "bigint", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    StorageLocationId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_request_reservations", x => x.id);
                    table.CheckConstraint("ck_stock_reservation_expires_after_reserved", "expires_at IS NULL OR expires_at >= reserved_at");
                    table.CheckConstraint("ck_stock_reservation_item_type_fields", "( (item_type = 'MATERIAL' AND material_id IS NOT NULL AND reserved_quantity IS NOT NULL AND reserved_quantity > 0 AND tool_asset_id IS NULL) OR  (item_type = 'TOOL' AND tool_asset_id IS NOT NULL AND material_id IS NULL AND reserved_quantity IS NULL))");
                    table.ForeignKey(
                        name: "FK_stock_request_reservations_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_request_reservations_stock_request_items_stock_request_item_id",
                        column: x => x.stock_request_item_id,
                        principalTable: "stock_request_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stock_request_reservations_storage_locations_StorageLocationId1",
                        column: x => x.StorageLocationId1,
                        principalTable: "storage_locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stock_request_reservations_storage_locations_storage_location_id",
                        column: x => x.storage_location_id,
                        principalTable: "storage_locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_request_reservations_tool_assets_tool_asset_id",
                        column: x => x.tool_asset_id,
                        principalTable: "tool_assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_request_reservations_users_reserved_by_user_id",
                        column: x => x.reserved_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activity_log_action_type",
                table: "activity_log",
                column: "action_type");

            migrationBuilder.CreateIndex(
                name: "IX_activity_log_created_at",
                table: "activity_log",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_activity_log_created_by_user_id",
                table: "activity_log",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_activity_log_entity_type_entity_id_created_at",
                table: "activity_log",
                columns: new[] { "entity_type", "entity_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_country_postal_code_city",
                table: "addresses",
                columns: new[] { "country", "postal_code", "city" });

            migrationBuilder.CreateIndex(
                name: "IX_billing_rates_is_active",
                table: "billing_rates",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_billing_rates_project_id",
                table: "billing_rates",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_billing_rates_project_id_is_active_valid_from_valid_to",
                table: "billing_rates",
                columns: new[] { "project_id", "is_active", "valid_from", "valid_to" });

            migrationBuilder.CreateIndex(
                name: "IX_billing_rates_valid_from",
                table: "billing_rates",
                column: "valid_from");

            migrationBuilder.CreateIndex(
                name: "IX_billing_rates_valid_to",
                table: "billing_rates",
                column: "valid_to");

            migrationBuilder.CreateIndex(
                name: "IX_calculators_code",
                table: "calculators",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_calculators_is_active",
                table: "calculators",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_customer_contacts_customer_id",
                table: "customer_contacts",
                column: "customer_id",
                unique: true,
                filter: "is_primary = 1");

            migrationBuilder.CreateIndex(
                name: "IX_customer_contacts_customer_id_is_primary",
                table: "customer_contacts",
                columns: new[] { "customer_id", "is_primary" });

            migrationBuilder.CreateIndex(
                name: "IX_customer_contacts_email",
                table: "customer_contacts",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_customer_notes_created_by_user_id",
                table: "customer_notes",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_notes_customer_id",
                table: "customer_notes",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_notes_customer_id_created_at",
                table: "customer_notes",
                columns: new[] { "customer_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_customer_notes_note_type",
                table: "customer_notes",
                column: "note_type");

            migrationBuilder.CreateIndex(
                name: "IX_customers_customer_type",
                table: "customers",
                column: "customer_type");

            migrationBuilder.CreateIndex(
                name: "IX_customers_name",
                table: "customers",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_customers_status",
                table: "customers",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_customers_tax_id",
                table: "customers",
                column: "tax_id",
                unique: true,
                filter: "[tax_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_employee_person_id",
                table: "employee",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_status",
                table: "employee",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_employee_bank_accounts_employee_id",
                table: "employee_bank_accounts",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_bank_accounts_employee_id_is_primary",
                table: "employee_bank_accounts",
                columns: new[] { "employee_id", "is_primary" });

            migrationBuilder.CreateIndex(
                name: "IX_employee_compensations_employee_id",
                table: "employee_compensations",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_compensations_employee_id_valid_from",
                table: "employee_compensations",
                columns: new[] { "employee_id", "valid_from" });

            migrationBuilder.CreateIndex(
                name: "IX_entity_addresses_address_id",
                table: "entity_addresses",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_entity_addresses_address_id_entity_type_entity_id_address_type",
                table: "entity_addresses",
                columns: new[] { "address_id", "entity_type", "entity_id", "address_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_entity_addresses_entity_type_entity_id",
                table: "entity_addresses",
                columns: new[] { "entity_type", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "IX_entity_addresses_entity_type_entity_id_address_type",
                table: "entity_addresses",
                columns: new[] { "entity_type", "entity_id", "address_type" });

            migrationBuilder.CreateIndex(
                name: "IX_invoice_history_changed_at",
                table: "invoice_history",
                column: "changed_at");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_history_changed_by_user_id",
                table: "invoice_history",
                column: "changed_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_history_invoice_id",
                table: "invoice_history",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_history_invoice_id_changed_at",
                table: "invoice_history",
                columns: new[] { "invoice_id", "changed_at" });

            migrationBuilder.CreateIndex(
                name: "IX_invoice_item_sources_invoice_item_id",
                table: "invoice_item_sources",
                column: "invoice_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_item_sources_source_type_source_id",
                table: "invoice_item_sources",
                columns: new[] { "source_type", "source_id" });

            migrationBuilder.CreateIndex(
                name: "IX_invoice_items_invoice_id",
                table: "invoice_items",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_items_invoice_id_sort_order",
                table: "invoice_items",
                columns: new[] { "invoice_id", "sort_order" });

            migrationBuilder.CreateIndex(
                name: "IX_invoices_created_by_user_id",
                table: "invoices",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_customer_id",
                table: "invoices",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_customer_id_issue_date",
                table: "invoices",
                columns: new[] { "customer_id", "issue_date" });

            migrationBuilder.CreateIndex(
                name: "IX_invoices_invoice_number",
                table: "invoices",
                column: "invoice_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invoices_issue_date",
                table: "invoices",
                column: "issue_date");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_issued_by_user_id",
                table: "invoices",
                column: "issued_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_project_id",
                table: "invoices",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_status",
                table: "invoices",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_attachments_article_id",
                table: "knowledge_article_attachments",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_attachments_article_id_sort_order",
                table: "knowledge_article_attachments",
                columns: new[] { "article_id", "sort_order" });

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_favorites_article_id_user_id",
                table: "knowledge_article_favorites",
                columns: new[] { "article_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_favorites_user_id",
                table: "knowledge_article_favorites",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_reads_article_id_user_id",
                table: "knowledge_article_reads",
                columns: new[] { "article_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_reads_read_at",
                table: "knowledge_article_reads",
                column: "read_at");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_article_reads_user_id",
                table: "knowledge_article_reads",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_category_id",
                table: "knowledge_articles",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_created_at",
                table: "knowledge_articles",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_created_by_user_id",
                table: "knowledge_articles",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_published_at",
                table: "knowledge_articles",
                column: "published_at");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_severity_tag",
                table: "knowledge_articles",
                column: "severity_tag");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_status",
                table: "knowledge_articles",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_status_published_at",
                table: "knowledge_articles",
                columns: new[] { "status", "published_at" });

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_articles_updated_by_user_id",
                table: "knowledge_articles",
                column: "updated_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_categories_name",
                table: "knowledge_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_knowledge_categories_parent_id",
                table: "knowledge_categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_leave_requests_approved_by_user_id",
                table: "leave_requests",
                column: "approved_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_leave_requests_employee_id",
                table: "leave_requests",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_leave_requests_employee_id_start_date",
                table: "leave_requests",
                columns: new[] { "employee_id", "start_date" });

            migrationBuilder.CreateIndex(
                name: "IX_leave_requests_requested_by_user_id",
                table: "leave_requests",
                column: "requested_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_leave_requests_start_date",
                table: "leave_requests",
                column: "start_date");

            migrationBuilder.CreateIndex(
                name: "IX_leave_requests_status",
                table: "leave_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_material_categories_name",
                table: "material_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_material_categories_parent_id",
                table: "material_categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_images_material_id",
                table: "material_images",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_images_material_id_sort_order",
                table: "material_images",
                columns: new[] { "material_id", "sort_order" });

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_created_by_user_id",
                table: "material_movements",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_from_storage_location_id",
                table: "material_movements",
                column: "from_storage_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_material_id",
                table: "material_movements",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_material_id_occurred_at",
                table: "material_movements",
                columns: new[] { "material_id", "occurred_at" });

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_occurred_at",
                table: "material_movements",
                column: "occurred_at");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_to_employee_id",
                table: "material_movements",
                column: "to_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_to_project_id",
                table: "material_movements",
                column: "to_project_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_to_storage_location_id",
                table: "material_movements",
                column: "to_storage_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_movements_to_vehicle_id",
                table: "material_movements",
                column: "to_vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_price_lists_material_id",
                table: "material_price_lists",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_price_lists_material_id_valid_from",
                table: "material_price_lists",
                columns: new[] { "material_id", "valid_from" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_material_project_balance_project_id",
                table: "material_project_balance",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_stock_material_id_storage_location_id",
                table: "material_stock",
                columns: new[] { "material_id", "storage_location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_material_stock_storage_location_id",
                table: "material_stock",
                column: "storage_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_vehicle_balance_vehicle_id",
                table: "material_vehicle_balance",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_materials_barcode",
                table: "materials",
                column: "barcode",
                unique: true,
                filter: "[barcode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_materials_category_id",
                table: "materials",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_materials_name",
                table: "materials",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_materials_sku",
                table: "materials",
                column: "sku",
                unique: true,
                filter: "[sku] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_created_at",
                table: "notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_created_by_user_id",
                table: "notifications",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_related_project_id",
                table: "notifications",
                column: "related_project_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_related_purchase_request_id",
                table: "notifications",
                column: "related_purchase_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_related_task_id",
                table: "notifications",
                column: "related_task_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_related_tool_asset_id",
                table: "notifications",
                column: "related_tool_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_related_vehicle_id",
                table: "notifications",
                column: "related_vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_severity",
                table: "notifications",
                column: "severity");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_type",
                table: "notifications",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_user_id",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_user_id_is_read_created_at",
                table: "notifications",
                columns: new[] { "user_id", "is_read", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_person_last_name",
                table: "person",
                column: "last_name");

            migrationBuilder.CreateIndex(
                name: "IX_person_user_id",
                table: "person",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_person_identifiers_person_id",
                table: "person_identifiers",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_identifiers_person_id_type",
                table: "person_identifiers",
                columns: new[] { "person_id", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_project_access_employee_id",
                table: "project_access",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_access_project_id",
                table: "project_access",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_access_project_id_employee_id",
                table: "project_access",
                columns: new[] { "project_id", "employee_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_project_access_project_id_is_enabled",
                table: "project_access",
                columns: new[] { "project_id", "is_enabled" });

            migrationBuilder.CreateIndex(
                name: "IX_project_attachments_created_by_user_idW",
                table: "project_attachments",
                column: "created_by_user_idW");

            migrationBuilder.CreateIndex(
                name: "IX_project_attachments_project_id",
                table: "project_attachments",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_attachments_project_id_created_at",
                table: "project_attachments",
                columns: new[] { "project_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_project_attachments_work_session_id",
                table: "project_attachments",
                column: "work_session_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_customer_contacts_customer_contact_id",
                table: "project_customer_contacts",
                column: "customer_contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_customer_contacts_project_id",
                table: "project_customer_contacts",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_customer_contacts_project_id_customer_contact_id",
                table: "project_customer_contacts",
                columns: new[] { "project_id", "customer_contact_id" });

            migrationBuilder.CreateIndex(
                name: "IX_project_customers_customer_id",
                table: "project_customers",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_customers_project_id",
                table: "project_customers",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_customers_project_id_customer_id",
                table: "project_customers",
                columns: new[] { "project_id", "customer_id" });

            migrationBuilder.CreateIndex(
                name: "IX_project_notes_created_by_user_id",
                table: "project_notes",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_notes_project_id",
                table: "project_notes",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_notes_project_id_created_at",
                table: "project_notes",
                columns: new[] { "project_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_project_notes_work_session_id",
                table: "project_notes",
                column: "work_session_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_assigned_to_employee_id",
                table: "project_tasks",
                column: "assigned_to_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_created_by_user_id",
                table: "project_tasks",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_due_at",
                table: "project_tasks",
                column: "due_at");

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_project_id",
                table: "project_tasks",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_project_id_status",
                table: "project_tasks",
                columns: new[] { "project_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_project_tasks_status",
                table: "project_tasks",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_projects_code",
                table: "projects",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_projects_created_by_user_id",
                table: "projects",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_responsible_employee_id",
                table: "projects",
                column: "responsible_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_start_date",
                table: "projects",
                column: "start_date");

            migrationBuilder.CreateIndex(
                name: "IX_projects_status",
                table: "projects",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_request_items_item_type",
                table: "purchase_request_items",
                column: "item_type");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_request_items_material_id",
                table: "purchase_request_items",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_request_items_purchase_request_id",
                table: "purchase_request_items",
                column: "purchase_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_request_items_tool_id",
                table: "purchase_request_items",
                column: "tool_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requests_approved_by_user_id",
                table: "purchase_requests",
                column: "approved_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requests_created_at",
                table: "purchase_requests",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requests_priority",
                table: "purchase_requests",
                column: "priority");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requests_project_id",
                table: "purchase_requests",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requests_project_id_status",
                table: "purchase_requests",
                columns: new[] { "project_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requests_requested_by_user_id",
                table: "purchase_requests",
                column: "requested_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requests_status",
                table: "purchase_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_service_order_history_changed_at",
                table: "service_order_history",
                column: "changed_at");

            migrationBuilder.CreateIndex(
                name: "IX_service_order_history_changed_by_user_id",
                table: "service_order_history",
                column: "changed_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_order_history_service_order_id",
                table: "service_order_history",
                column: "service_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_order_history_service_order_id_changed_at",
                table: "service_order_history",
                columns: new[] { "service_order_id", "changed_at" });

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_appointment_at",
                table: "service_orders",
                column: "appointment_at");

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_approval_status",
                table: "service_orders",
                column: "approval_status");

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_approved_by_user_id",
                table: "service_orders",
                column: "approved_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_completed_by_user_id",
                table: "service_orders",
                column: "completed_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_priority",
                table: "service_orders",
                column: "priority");

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_requested_at",
                table: "service_orders",
                column: "requested_at");

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_requested_by_user_id",
                table: "service_orders",
                column: "requested_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_status",
                table: "service_orders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_vehicle_id",
                table: "service_orders",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_fulfillment_items_from_storage_location_id",
                table: "stock_request_fulfillment_items",
                column: "from_storage_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_fulfillment_items_fulfillment_id",
                table: "stock_request_fulfillment_items",
                column: "fulfillment_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_fulfillment_items_item_type",
                table: "stock_request_fulfillment_items",
                column: "item_type");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_fulfillment_items_material_id",
                table: "stock_request_fulfillment_items",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_fulfillment_items_tool_asset_id",
                table: "stock_request_fulfillment_items",
                column: "tool_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_fulfillments_issued_at",
                table: "stock_request_fulfillments",
                column: "issued_at");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_fulfillments_issued_by_user_id",
                table: "stock_request_fulfillments",
                column: "issued_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_fulfillments_stock_request_id",
                table: "stock_request_fulfillments",
                column: "stock_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_history_changed_at",
                table: "stock_request_history",
                column: "changed_at");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_history_changed_by_user_id",
                table: "stock_request_history",
                column: "changed_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_history_stock_request_id",
                table: "stock_request_history",
                column: "stock_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_history_stock_request_id_changed_at",
                table: "stock_request_history",
                columns: new[] { "stock_request_id", "changed_at" });

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_items_item_type",
                table: "stock_request_items",
                column: "item_type");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_items_material_id",
                table: "stock_request_items",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_items_stock_request_id",
                table: "stock_request_items",
                column: "stock_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_items_tool_id",
                table: "stock_request_items",
                column: "tool_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_reservations_material_id",
                table: "stock_request_reservations",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_reservations_reserved_at",
                table: "stock_request_reservations",
                column: "reserved_at");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_reservations_reserved_by_user_id",
                table: "stock_request_reservations",
                column: "reserved_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_reservations_status",
                table: "stock_request_reservations",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_reservations_stock_request_item_id",
                table: "stock_request_reservations",
                column: "stock_request_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_reservations_storage_location_id",
                table: "stock_request_reservations",
                column: "storage_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_reservations_StorageLocationId1",
                table: "stock_request_reservations",
                column: "StorageLocationId1");

            migrationBuilder.CreateIndex(
                name: "IX_stock_request_reservations_tool_asset_id",
                table: "stock_request_reservations",
                column: "tool_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_requests_approved_by_user_id",
                table: "stock_requests",
                column: "approved_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_requests_employee_id",
                table: "stock_requests",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_requests_priority",
                table: "stock_requests",
                column: "priority");

            migrationBuilder.CreateIndex(
                name: "IX_stock_requests_project_id",
                table: "stock_requests",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_requests_requested_at",
                table: "stock_requests",
                column: "requested_at");

            migrationBuilder.CreateIndex(
                name: "IX_stock_requests_requested_by_user_id",
                table: "stock_requests",
                column: "requested_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_requests_status",
                table: "stock_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_stock_requests_vehicle_id",
                table: "stock_requests",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_locations_code",
                table: "storage_locations",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_attachments_task_id",
                table: "task_attachments",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_attachments_task_id_created_at",
                table: "task_attachments",
                columns: new[] { "task_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_task_updates_created_by_user_id",
                table: "task_updates",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_updates_task_id_created_at",
                table: "task_updates",
                columns: new[] { "task_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_tool_asset_stock_storage_location_id",
                table: "tool_asset_stock",
                column: "storage_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assets_asset_tag",
                table: "tool_assets",
                column: "asset_tag",
                unique: true,
                filter: "[asset_tag] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assets_condition",
                table: "tool_assets",
                column: "condition");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assets_serial_number",
                table: "tool_assets",
                column: "serial_number",
                unique: true,
                filter: "[serial_number] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assets_status",
                table: "tool_assets",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assets_tool_id",
                table: "tool_assets",
                column: "tool_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_assigned_at",
                table: "tool_assignments",
                column: "assigned_at");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_created_by_user_id",
                table: "tool_assignments",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_returned_at",
                table: "tool_assignments",
                column: "returned_at");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_to_employee_id",
                table: "tool_assignments",
                column: "to_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_to_project_id",
                table: "tool_assignments",
                column: "to_project_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_to_storage_location_id",
                table: "tool_assignments",
                column: "to_storage_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_to_vehicle_id",
                table: "tool_assignments",
                column: "to_vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_assignments_tool_asset_id",
                table: "tool_assignments",
                column: "tool_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_categories_name",
                table: "tool_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tool_categories_parent_id",
                table: "tool_categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_images_tool_id",
                table: "tool_images",
                column: "tool_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_images_tool_id_sort_order",
                table: "tool_images",
                columns: new[] { "tool_id", "sort_order" });

            migrationBuilder.CreateIndex(
                name: "IX_tool_issue_attachments_tool_issue_id",
                table: "tool_issue_attachments",
                column: "tool_issue_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_issues_created_at",
                table: "tool_issues",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_tool_issues_reported_by_user_id",
                table: "tool_issues",
                column: "reported_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_issues_status",
                table: "tool_issues",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_tool_issues_tool_asset_id",
                table: "tool_issues",
                column: "tool_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_tool_issues_tool_asset_id_status",
                table: "tool_issues",
                columns: new[] { "tool_asset_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_tools_barcode",
                table: "tools",
                column: "barcode",
                unique: true,
                filter: "[barcode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tools_category_id",
                table: "tools",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_tools_is_active",
                table: "tools",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_tools_name",
                table: "tools",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tools_sku",
                table: "tools",
                column: "sku",
                unique: true,
                filter: "[sku] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_is_active",
                table: "users",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId1",
                table: "users",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_assignments_employee_id",
                table: "vehicle_assignments",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_assignments_vehicle_id",
                table: "vehicle_assignments",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_assignments_vehicle_id_assigned_at",
                table: "vehicle_assignments",
                columns: new[] { "vehicle_id", "assigned_at" });

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issue_attachments_vehicle_issue_id",
                table: "vehicle_issue_attachments",
                column: "vehicle_issue_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issues_created_at",
                table: "vehicle_issues",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issues_reported_by_user_id",
                table: "vehicle_issues",
                column: "reported_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issues_status",
                table: "vehicle_issues",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issues_vehicle_id",
                table: "vehicle_issues",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_issues_vehicle_id_status",
                table: "vehicle_issues",
                columns: new[] { "vehicle_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_mileage_records_created_by_user_id",
                table: "vehicle_mileage_records",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_mileage_records_recorded_at",
                table: "vehicle_mileage_records",
                column: "recorded_at");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_mileage_records_vehicle_id",
                table: "vehicle_mileage_records",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_mileage_records_vehicle_id_recorded_at",
                table: "vehicle_mileage_records",
                columns: new[] { "vehicle_id", "recorded_at" });

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_plate_number",
                table: "vehicles",
                column: "plate_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_status",
                table: "vehicles",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_vin",
                table: "vehicles",
                column: "vin",
                unique: true,
                filter: "[vin] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_work_session_events_created_by_user_id",
                table: "work_session_events",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_work_session_events_work_session_id",
                table: "work_session_events",
                column: "work_session_id");

            migrationBuilder.CreateIndex(
                name: "IX_work_session_events_work_session_id_event_at",
                table: "work_session_events",
                columns: new[] { "work_session_id", "event_at" });

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_employee_id",
                table: "work_sessions",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_employee_id_project_id_started_at",
                table: "work_sessions",
                columns: new[] { "employee_id", "project_id", "started_at" });

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_project_id",
                table: "work_sessions",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_started_at",
                table: "work_sessions",
                column: "started_at");

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_started_by_user_id",
                table: "work_sessions",
                column: "started_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_work_sessions_status",
                table: "work_sessions",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_log");

            migrationBuilder.DropTable(
                name: "billing_rates");

            migrationBuilder.DropTable(
                name: "calculators");

            migrationBuilder.DropTable(
                name: "customer_notes");

            migrationBuilder.DropTable(
                name: "employee_bank_accounts");

            migrationBuilder.DropTable(
                name: "employee_compensations");

            migrationBuilder.DropTable(
                name: "entity_addresses");

            migrationBuilder.DropTable(
                name: "invoice_history");

            migrationBuilder.DropTable(
                name: "invoice_item_sources");

            migrationBuilder.DropTable(
                name: "knowledge_article_attachments");

            migrationBuilder.DropTable(
                name: "knowledge_article_favorites");

            migrationBuilder.DropTable(
                name: "knowledge_article_reads");

            migrationBuilder.DropTable(
                name: "leave_requests");

            migrationBuilder.DropTable(
                name: "material_images");

            migrationBuilder.DropTable(
                name: "material_movements");

            migrationBuilder.DropTable(
                name: "material_price_lists");

            migrationBuilder.DropTable(
                name: "material_project_balance");

            migrationBuilder.DropTable(
                name: "material_stock");

            migrationBuilder.DropTable(
                name: "material_vehicle_balance");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "person_identifiers");

            migrationBuilder.DropTable(
                name: "project_access");

            migrationBuilder.DropTable(
                name: "project_attachments");

            migrationBuilder.DropTable(
                name: "project_customer_contacts");

            migrationBuilder.DropTable(
                name: "project_customers");

            migrationBuilder.DropTable(
                name: "project_notes");

            migrationBuilder.DropTable(
                name: "purchase_request_items");

            migrationBuilder.DropTable(
                name: "service_order_history");

            migrationBuilder.DropTable(
                name: "stock_request_fulfillment_items");

            migrationBuilder.DropTable(
                name: "stock_request_history");

            migrationBuilder.DropTable(
                name: "stock_request_reservations");

            migrationBuilder.DropTable(
                name: "task_attachments");

            migrationBuilder.DropTable(
                name: "task_updates");

            migrationBuilder.DropTable(
                name: "tool_asset_stock");

            migrationBuilder.DropTable(
                name: "tool_assignments");

            migrationBuilder.DropTable(
                name: "tool_images");

            migrationBuilder.DropTable(
                name: "tool_issue_attachments");

            migrationBuilder.DropTable(
                name: "vehicle_assignments");

            migrationBuilder.DropTable(
                name: "vehicle_issue_attachments");

            migrationBuilder.DropTable(
                name: "vehicle_mileage_records");

            migrationBuilder.DropTable(
                name: "work_session_events");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "invoice_items");

            migrationBuilder.DropTable(
                name: "knowledge_articles");

            migrationBuilder.DropTable(
                name: "customer_contacts");

            migrationBuilder.DropTable(
                name: "purchase_requests");

            migrationBuilder.DropTable(
                name: "service_orders");

            migrationBuilder.DropTable(
                name: "stock_request_fulfillments");

            migrationBuilder.DropTable(
                name: "stock_request_items");

            migrationBuilder.DropTable(
                name: "project_tasks");

            migrationBuilder.DropTable(
                name: "storage_locations");

            migrationBuilder.DropTable(
                name: "tool_issues");

            migrationBuilder.DropTable(
                name: "vehicle_issues");

            migrationBuilder.DropTable(
                name: "work_sessions");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "knowledge_categories");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "stock_requests");

            migrationBuilder.DropTable(
                name: "tool_assets");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "material_categories");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "tools");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "tool_categories");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
