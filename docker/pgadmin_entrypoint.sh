#!/bin/sh

# pgAdmin の servers.json テンプレートファイル
TEMPLATE_FILE="/pgadmin4/servers.json.template" # コンテナ内のパス
OUTPUT_DIR="/var/lib/pgadmin/servers"          # 永続ボリューム内の出力ディレクトリ
OUTPUT_FILE="$OUTPUT_DIR/servers.json"         # 生成される servers.json の最終パス

# 環境変数をチェック
if [ -z "$DATABASE_NAME" ]; then
  echo "Error: DATABASE_NAME environment variable is not set."
  exit 1
fi
if [ -z "$DATABASE_USER" ]; then
  echo "Error: DATABASE_USER environment variable is not set."
  exit 1
fi

# 出力ディレクトリが存在しない場合は作成
mkdir -p "$OUTPUT_DIR"

echo "Generating servers.json from template..."

# sed を使ってテンプレートのプレースホルダーを環境変数で置換
sed -e "s/__DATABASE_NAME__/${DATABASE_NAME}/g" \
    -e "s/__DATABASE_USER__/${DATABASE_USER}/g" \
    "$TEMPLATE_FILE" > "$OUTPUT_FILE"

echo "servers.json generated successfully at $OUTPUT_FILE."

# pgAdmin のオリジナルのエントリーポイントを実行
exec /entrypoint.sh "$@"