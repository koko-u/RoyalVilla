UPDATE "villas"
SET "name"        = @Name,
    "details"     = @Details,
    "rate"        = @Rate,
    "square_feet" = @SquareFeet,
    "occupancy"   = @Occupancy,
    "image_url"   = @ImageUrl
WHERE "id" = @Id
RETURNING "id",
    "name",
    "details",
    "rate",
    "square_feet",
    "occupancy",
    "image_url";