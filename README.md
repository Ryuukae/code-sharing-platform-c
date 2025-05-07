# Code Sharing Platform

This is a simple Code Sharing Platform built with ASP.NET Core. It provides both a web interface and a RESTful API to create, view, and retrieve code snippets.

## Prerequisites

- [.NET 6 SDK or later](https://dotnet.microsoft.com/download)

## Building and Running

1. Clone the repository.
2. Navigate to the project directory.
3. Build the project:

```bash
dotnet build
```

4. Run the project:

```bash
dotnet run
```

The application will start and listen on `http://0.0.0.0:5000`.

## API Usage

The API base URL is: `http://localhost:5000/api/code`

### Create a New Code Snippet

- **Endpoint:** `POST /api/code/new`
- **Description:** Creates a new code snippet.
- **Request Body:** JSON representation of a `CodeSnippet` object.

Example request:

```bash
curl -X POST http://localhost:5000/api/code/new \
  -H "Content-Type: application/json" \
  -d '{
    "ID": "unique-snippet-id",
    "Content": "Console.WriteLine(\"Hello, world!\");",
    "Name": "Sample snippet",
    "Type": "Basic"
  }'
```

Example response:

```json
{
  "Id": "unique-snippet-id"
}
```

### Get a Code Snippet by ID

- **Endpoint:** `GET /api/code/{id}`
- **Description:** Retrieves a code snippet by its unique ID.

Example request:

```bash
curl http://localhost:5000/api/code/unique-snippet-id
```

Example response:

```json
{
  "ID": "unique-snippet-id",
  "Content": "Console.WriteLine(\"Hello, world!\");",
  "Name": "Sample snippet",
  "CreationTimestamp": "2024-06-01T12:00:00Z",
  "Type": "Basic"
}
```

### Get Latest Code Snippets

- **Endpoint:** `GET /api/code/latest`
- **Description:** Retrieves a list of the latest code snippets.

Example request:

```bash
curl http://localhost:5000/api/code/latest
```

Example response:

```json
[
  {
    "ID": "unique-snippet-id-1",
    "Content": "Console.WriteLine(\"Hello, world!\");",
    "Name": "Sample snippet 1",
    "CreationTimestamp": "2024-06-01T12:00:00Z",
    "Type": "Basic"
  },
  {
    "ID": "unique-snippet-id-2",
    "Content": "print('Hello, world!')",
    "Name": "Sample snippet 2",
    "CreationTimestamp": "2024-06-01T12:05:00Z",
    "Type": "Expiring"
  }
]
```

## Web Interface

The application also provides a web interface accessible via a browser:

- View latest snippets: `http://localhost:5000/code/latest`
- View a specific snippet: `http://localhost:5000/code/view/{id}`
- Create a new snippet: `http://localhost:5000/code/new`

## License

This project is licensed under the MIT License.
