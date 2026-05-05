INSERT INTO "villas" ("name",
                      "details",
                      "rate",
                      "square_feet",
                      "occupancy",
                      "image_url")
VALUES (@Name,
        @Details,
        @Rate,
        @SquareFeet,
        @Occupancy,
        @ImageUrl)
RETURNING "id",
    "name",
    "details",
    "rate",
    "square_feet",
    "occupancy",
    "image_url";