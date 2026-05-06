WITH "user_row" AS (SELECT "id",
                           "email",
                           "display_name",
                           "is_active"
                    FROM "users"
                    WHERE "email" = @Email)
SELECT "U"."id"           AS "id",
       "U"."email"        AS "email",
       "U"."display_name" AS "display_name",
       "U"."is_active"    AS "is_active",
       "R"."id"           AS "role_id",
       "R"."name"         AS "role_name"
FROM "user_row" AS "U"
         LEFT OUTER JOIN
     "user_roles" AS "UR"
     ON "U"."id" = "UR"."user_id"
         LEFT OUTER JOIN
     "roles" AS "R"
     ON "UR"."role_id" = "R"."id";