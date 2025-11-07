#!/bin/bash

echo "🧪 Testing Webhook with REAL SePay format (no dashes in GUID)"
echo "=============================================================="
echo ""

# GUID from MongoDB (with dashes)
BILL_ID_MONGO="148a4d2e-8ed5-4d16-abea-10d3974e288f"

# GUID in webhook (bank removes dashes)
BILL_ID_WEBHOOK="148a4d2e8ed54d16abea10d3974e288f"

WEBHOOK_URL="http://localhost:7212/api/Payment/webhook/sepay"

echo "📋 Bill ID (MongoDB):  $BILL_ID_MONGO"
echo "📋 Bill ID (Webhook):  $BILL_ID_WEBHOOK"
echo "🌐 Webhook URL: $WEBHOOK_URL"
echo ""

echo "📤 Sending webhook with REAL format (GUID without dashes)..."
curl -X POST "$WEBHOOK_URL" \
  -H "Content-Type: application/json" \
  -d "{
    \"gateway\": \"MBBank\",
    \"transactionDate\": \"$(date '+%Y-%m-%d %H:%M:%S')\",
    \"accountNumber\": \"0799568616\",
    \"content\": \"MBVCB.11537003260.941970.Thanh toan hoa don $BILL_ID_WEBHOOK.CT tu 1051396698 NGUYEN QUOC HIEN\",
    \"transferType\": \"in\",
    \"transferAmount\": 10000,
    \"referenceCode\": \"FT25305Z15289052\",
    \"accumulated\": 0,
    \"id\": \"TEST_$(date +%s)\"
  }"

echo ""
echo ""
echo "✅ Done! Check PaymentAPI logs"
echo ""
echo "Expected: Backend should:"
echo "  1. Extract: $BILL_ID_WEBHOOK"
echo "  2. Convert to: $BILL_ID_MONGO"
echo "  3. Find bill and create payment"
echo "  4. Update bill status to Paid"
