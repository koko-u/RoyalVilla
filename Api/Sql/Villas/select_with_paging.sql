SELECT
    "id",
    "name",
    "details",
    "rate",
    "square_feet",
    "occupancy",
    "image_url"
FROM "villas"
ORDER BY "id"
LIMIT @Limit OFFSET @Offset;