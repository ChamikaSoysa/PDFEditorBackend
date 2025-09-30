PDF Editor - Backend

A ASP.NET Core backend for PDF processing and manipulation, providing RESTful APIs for PDF editing, metadata management, and format conversion.

------------------------------------------------------------------------------------------------------------------

Project Structure

PDFBackend/
├── Controllers/
│   ├── HomeController.cs          # Default home controller
│   └── PdfController.cs           # Main PDF processing API endpoints
├── Services/
│   ├── PdfService.cs              # Core PDF manipulation logic
│   └── FileStorageService.cs      # File upload and storage management
├── Interfaces/
│   ├── IPdfService.cs             # PDF service contract interface
│   └── IFileStorageService.cs     # File storage contract interface
├── DTOs/ (Data Transfer Objects)
│   ├── EditMetadataRequest.cs     # Metadata update request model
│   ├── EditTextRequest.cs         # Text editing request model
│   ├── FilePathRequest.cs         # File path request model
│   ├── MetadataDto.cs             # Metadata data transfer object
│   └── TextEditDto.cs             # Text edit data transfer object
├── Middlewares/
│   ├── GlobalExceptionMiddleware.cs # Global error handling
│   └── RequestResponseLoggingMiddleware.cs # Request logging
├── Validators/
│   └── TextEditValidator.cs       # FluentValidation rules for text edits
├── Properties/
│   └── launchSettings.json        # ASP.NET Core launch configuration
├── logs/                          # Serilog generated log files
├── appsettings.json               # Application configuration
└── Program.cs                     # Application entry point

-----------------------------------------------------------------------------------------------------------------

Prerequisites

-NET 8.0 SDK

-Aspose.PDF and Aspose.Words licenses (for production)

----------------------------------------------------------------------------------------------------------------

Installation

# Navigate to backend directory
cd PDFBackend

# Restore NuGet packages
dotnet restore

# Run the application
dotnet run

# Access API at: https://localhost:7200
# Swagger UI at: https://localhost:7200/swagger

---------------------------------------------------------------------------------------------------------------

Packages Used 

Core Framework---------------------------------------

*ASP.NET Core 8.0 - High-performance web framework

*C# - Primary programming language

PDF Processing---------------------------------------

*Aspose.PDF - Advanced PDF manipulation

*Aspose.Words - Document processing and conversion

Supporting Libraries---------------------------------

*FluentValidation.AspNetCore - Robust input validation

*Serilog.AspNetCore - Structured logging

---------------------------------------------------------------------------------------------------------------

API Endpoints 

PDF Management Endpoints

Method	Endpoint	              Description	                Request Body
POST	/api/pdf/upload	          Upload PDF file (10MB max)	FormData with file
POST	/api/pdf/preview	      Get PDF preview	            { "filePath": "string" }
POST	/api/pdf/edit-text	      Add text overlays to PDF	    { "filePath": "string", "edits": object }
POST	/api/pdf/edit-metadata    Update PDF metadata	        { "filePath": "string", "metadata": object }
GET	    /api/pdf/export/{format}  Export to PDF/DOCX/Images	    Query: filePath

---------------------------------------------------------------------------------------------------------------

Core Services

PdfService (Services/PdfService.cs)---------------------------------

Text Overlay Placement - Add text to specific PDF coordinates

Metadata Updates - Modify PDF document properties

Format Conversion - PDF to DOCX and PNG conversion

Image Processing - Convert PDF pages to images with configurable resolution

ZIP Archive Creation - Package multiple images into downloadable ZIP

FileStorageService (Services/FileStorageService.cs)-----------------

Secure File Upload - PDF-only validation with sanitization

Temporary Storage - GUID-based directory structure in system temp

File Management - Read, write, and delete operations with cleanup

Security - Path traversal prevention and input sanitization

----------------------------------------------------------------------------------------------------------------

Configuration

Program.cs Setup

// Logging Configuration
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

// Service Registration
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});


-------------------------------------------------------------------------------------------------------------

Launch Settings (Properties/launchSettings.json)

HTTPS: https://localhost:7200

Development environment with Swagger UI enabled

--------------------------------------------------------------------------------------------------------------

Security & Validation

Input Validation---------------------------------------------------

File Type Validation - PDF-only uploads with MIME type checking

Size Limits - 10MB maximum file size enforcement

Coordinate Validation - X/Y coordinates within 0-10000 range

Text Length - 1-500 character limits for text overlays

Security Measures--------------------------------------------------

Input Sanitization - Comprehensive filename and content sanitization

CORS Policy - Restricted to frontend origin only

Exception Handling - Secure error messages without system exposure

Path Security - Prevention of directory traversal attacks

-----------------------------------------------------------------------------------------------------------------

📊 Logging & Monitoring

Serilog Configuration----------------------------------------

Console Output - Real-time development logging

File Logging - Daily rolling logs in logs/ directory

Structured Logging - JSON-formatted log entries for analysis


Custom Middleware--------------------------------------------
Request/Response Logging - Complete API call audit trails

Global Exception Handling - Consistent error management

Performance Monitoring - Request timing and resource usage

-----------------------------------------------------------------------------------------------------------------

 Assumptions

Infrastructure--------------------------------------------------------

**Temporary file storage in system temp directory is sufficient

**Aspose licenses are available and configured for production

* Sufficient disk space for file operations and processing

* Development environment properly isolated from production

File Processing-------------------------------------------------------
** PDF files are legitimate and not corrupted

* Text edits are within reasonable bounds (coordinates and quantity)

** File sizes up to 10MB can be processed efficiently

* Temporary files are properly cleaned up after processing

API Consumption--------------------------------------------------------
* Frontend runs on http://localhost:3000 during development

* All API requests come from trusted frontend sources

* Request rates are within reasonable infrastructure limits

Technical Implementation------------------------------------------------
* PDF coordinate system (bottom-left origin) correctly handled

* Text overlay positioning is accurate and consistent

* Metadata updates properly persist in PDF structure

* Format conversions maintain document integrity

* Image conversion resolution (150 DPI) provides good quality

Deployment-------------------------------------------------------------

Production Considerations

Set Aspose licenses in production environment

Configure proper CORS origins for production frontend

Set up persistent file storage or cloud storage

Configure Serilog for production logging levels

Enable HTTPS and proper certificate management

Set up health checks and monitoring