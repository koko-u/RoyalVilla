WITH "role_rows" AS (
    SELECT
        "id" AS "role_id",
        "name" AS "role_name"
    FROM "roles"
    WHERE "name" = ANY(@RoleNames)
)
INSERT INTO "user_roles" ("user_id", "role_id")
SELECT 
    @UserId,
    role_id
FROM "role_rows";