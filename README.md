# Task Management API

## Deskripsi Project

Task Management API adalah REST API berbasis ASP.NET Core yang digunakan untuk mengelola data tugas (task). API ini mendukung operasi CRUD (Create, Read, Update, Delete) dan terhubung dengan database PostgreSQL menggunakan relasi antar tabel (users, tasks, categories).

---

## Teknologi yang Digunakan

* **Bahasa**: C#
* **Framework**: ASP.NET Core Web API
* **Database**: PostgreSQL
* **Library**: Npgsql
* **Tools**: Visual Studio, Swagger, Postman

---

## Struktur Database

### 1. users

| Kolom      | Tipe        |
| ---------- | ----------- |
| id         | SERIAL (PK) |
| name       | VARCHAR     |
| email      | VARCHAR     |
| created_at | TIMESTAMP   |
| updated_at | TIMESTAMP   |

---

### 2. categories

| Kolom      | Tipe        |
| ---------- | ----------- |
| id         | SERIAL (PK) |
| name       | VARCHAR     |
| created_at | TIMESTAMP   |
| updated_at | TIMESTAMP   |

---

### 3. tasks

| Kolom       | Tipe                              |
| ----------- | --------------------------------- |
| id          | SERIAL (PK)                       |
| user_id     | INT (FK)                          |
| category_id | INT (FK)                          |
| title       | VARCHAR                           |
| description | TEXT                              |
| status      | VARCHAR (todo, in_progress, done) |
| created_at  | TIMESTAMP                         |
| updated_at  | TIMESTAMP                         |

---

## Relasi Tabel

* 1 user dapat memiliki banyak task
* 1 category dapat memiliki banyak task
* task terhubung ke user dan category menggunakan foreign key

---

## Langkah Instalasi dan Menjalankan Project

### 1. Clone Repository

```bash
https://github.com/Veynasti/Sistem-manajemen-tugas.git
```

### 2. Buka Project

* Buka menggunakan Visual Studio

### 3. Install Dependency

* Pastikan package seperti Npgsql sudah terinstall

### 4. Konfigurasi Koneksi

Edit file `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=task_management;Username=postgres;Password=yourpassword"
}
```

### 5. Jalankan Project

* Tekan **F5** atau klik Run
* Swagger akan terbuka otomatis

---

## Cara Import Database

1. Buka PostgreSQL / pgAdmin
2. Buat database baru:

```sql
CREATE DATABASE task_management;
```

3. Buka Query Tool
4. Jalankan file `database.sql`

---

## Daftar Endpoint

| Method | URL             | Keterangan                           |
| ------ | --------------- | ------------------------------------ |
| GET    | /api/tasks      | Mengambil semua data task            |
| GET    | /api/tasks/{id} | Mengambil detail task berdasarkan id |
| POST   | /api/tasks      | Menambahkan task baru                |
| PUT    | /api/tasks/{id} | Mengupdate data task berdasarkan id  |
| DELETE | /api/tasks/{id} | Menghapus task berdasarkan id        |

---

## Link video presentasi
https://youtu.be/08p2XVZVK5s?si=dkcUclLlbo3AQGiU

