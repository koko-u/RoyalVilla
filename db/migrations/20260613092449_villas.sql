-- migrate:up
CREATE TABLE IF NOT EXISTS "villas"
(
    "id"          UUID          NOT NULL DEFAULT "uuidv7"(),
    "name"        VARCHAR(255)  NOT NULL,
    "details"     TEXT          NULL     DEFAULT NULL,
    "rate"        DECIMAL(3, 2) NULL     DEFAULT NULL,
    "square_feet" INT           NULL     DEFAULT NULL,
    "occupancy"   INT           NULL     DEFAULT NULL,
    "image_url"   VARCHAR(255)  NULL     DEFAULT NULL,
    "created_at"  TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    "updated_at"  TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    CONSTRAINT "villas_pkey" PRIMARY KEY ("id")
);

CREATE INDEX IF NOT EXISTS "idx_villas_name" ON "villas" ("name");
CREATE INDEX IF NOT EXISTS "idx_villas_name_like" ON "villas" USING "gin" ("name" "gin_trgm_ops");

CREATE OR REPLACE TRIGGER "tgr_villas_updated_at"
    BEFORE UPDATE
    ON "villas"
    FOR EACH ROW
EXECUTE PROCEDURE "moddatetime"("updated_at");


-- migrate:down
DROP TABLE IF EXISTS "villas";
