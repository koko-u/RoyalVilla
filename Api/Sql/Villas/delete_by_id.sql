DELETE
FROM "villas"
WHERE "id" = @Id
RETURNING "id",
    "name",
    "details",
    "rate",
    "square_feet",
    "occupancy",
    "image_url";