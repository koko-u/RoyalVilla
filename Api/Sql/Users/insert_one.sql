INSERT INTO "users" ("email", "password", "display_name")
VALUES (@Email, @PasswordHash, @DisplayName)
RETURNING "id", 
    "email", 
    "display_name", 
    "is_active";