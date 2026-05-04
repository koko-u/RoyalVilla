-- migrate:up
CREATE TABLE IF NOT EXISTS "villas" (
    "id" INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY,
    "name" VARCHAR(255) NOT NULL,
    "details" TEXT NULL DEFAULT NULL,
    "rate" DECIMAL(5,2) NOT NULL DEFAULT 0.0,
    "square_feet" INTEGER NOT NULL DEFAULT 0,
    "occupancy" INTEGER NOT NULL DEFAULT 0,
    "image_url" VARCHAR(255) NULL DEFAULT NULL,
    "created_at" TIMESTAMPTZ NOT NULL DEFAULT now(),
    "updated_at" TIMESTAMPTZ NOT NULL DEFAULT now(),
    CONSTRAINT "villas_pkey" PRIMARY KEY ("id")
);

CREATE INDEX IF NOT EXISTS "idx_villas_name" ON "villas" ("name");
CREATE INDEX IF NOT EXISTS "idx_villas_name_like" ON "villas" USING gin ("name" gin_trgm_ops);
CREATE INDEX IF NOT EXISTS "idx_villas_rate" ON "villas" ("rate" DESC);

CREATE OR REPLACE TRIGGER "villas_updated_at"
    BEFORE UPDATE 
    ON "villas"
    FOR EACH ROW
    EXECUTE PROCEDURE moddatetime("updated_at");

COMMENT ON TABLE "villas" IS 'リゾートハウスの情報を保有します';
COMMENT ON COLUMN "villas"."name" IS '名称';
COMMENT ON COLUMN "villas"."details" IS '詳細情報';
COMMENT ON COLUMN "villas"."rate" IS '料金';
COMMENT ON COLUMN "villas"."square_feet" IS '広さ(平方フィート)';
COMMENT ON COLUMN "villas"."occupancy" IS '占有率';
COMMENT ON COLUMN "villas"."image_url" IS '画像';

-- migrate:down
DROP TABLE IF EXISTS "villas";
