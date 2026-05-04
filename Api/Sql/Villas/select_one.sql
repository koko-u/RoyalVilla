SELECT
    "id",
    "name",
    "details",
    "rate",
    "square_feet",
    "occupancy",
    "image_url"
FROM "villas"
WHERE "id" = @id;