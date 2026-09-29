using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class StoreFormFieldTypeAsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create a temporary string column
            migrationBuilder.AddColumn<string>(
                name: "FieldTypeTemp",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: true);

            // 2. Convert existing integer enum values to their string names
            migrationBuilder.Sql("""
                UPDATE FormFields
                SET FieldTypeTemp =
                    CASE FieldType
                        WHEN 0 THEN 'TEXT'
                        WHEN 1 THEN 'TEXTAREA'
                        WHEN 2 THEN 'NUMBER'
                        WHEN 3 THEN 'EMAIL'
                        WHEN 4 THEN 'DATE'
                        WHEN 5 THEN 'DROPDOWN'
                        WHEN 6 THEN 'RADIO'
                        WHEN 7 THEN 'CHECKBOX'
                    END
            """);

            // 3. Remove the old integer column
            migrationBuilder.DropColumn(
                name: "FieldType",
                table: "FormFields");

            // 4. Rename the temporary string column
            //    to the original column name
            migrationBuilder.RenameColumn(
                name: "FieldTypeTemp",
                table: "FormFields",
                newName: "FieldType");

            // 5. Make the new FieldType column required
            migrationBuilder.AlterColumn<string>(
                name: "FieldType",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Create a temporary integer column
            migrationBuilder.AddColumn<int>(
                name: "FieldTypeTemp",
                table: "FormFields",
                type: "int",
                nullable: true);

            // 2. Convert string values back to their enum integer values
            migrationBuilder.Sql("""
                UPDATE FormFields
                SET FieldTypeTemp =
                    CASE FieldType
                        WHEN 'TEXT' THEN 0
                        WHEN 'TEXTAREA' THEN 1
                        WHEN 'NUMBER' THEN 2
                        WHEN 'EMAIL' THEN 3
                        WHEN 'DATE' THEN 4
                        WHEN 'DROPDOWN' THEN 5
                        WHEN 'RADIO' THEN 6
                        WHEN 'CHECKBOX' THEN 7
                    END
            """);

            // 3. Remove the string column
            migrationBuilder.DropColumn(
                name: "FieldType",
                table: "FormFields");

            // 4. Rename the temporary integer column
            //    back to the original column name
            migrationBuilder.RenameColumn(
                name: "FieldTypeTemp",
                table: "FormFields",
                newName: "FieldType");

            // 5. Make the integer column required
            migrationBuilder.AlterColumn<int>(
                name: "FieldType",
                table: "FormFields",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}