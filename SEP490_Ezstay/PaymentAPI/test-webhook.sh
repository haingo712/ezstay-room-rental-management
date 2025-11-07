#!/bin/bash

echo "🧪 Testing Payment Webhook"
echo "=========================="
echo ""

BILL_ID="148a4d2e-8ed5-4d16-abea-10d3974e288f"
WEBHOOK_URL="http://localhost:7212/api/Payment/webhook/sepay"

echo "📋 Bill ID: $BILL_ID"
echo "🌐 Webhook URL: $WEBHOOK_URL"
echo ""

echo "📤 Sending webhook request..."
curl -X POST "$WEBHOOK_URL" \
  -H "Content-Type: application/json" \
  -d "{
    \"gateway\": \"MBBank\",
    \"transactionDate\": \"$(date '+%Y-%m-%d %H:%M:%S')\",
    \"accountNumber\": \"0799568616\",
    \"content\": \"Thanh toan hoa don $BILL_ID\",
    \"transferType\": \"in\",
    \"transferAmount\": 10000,
    \"id\": \"TEST_$(date +%s)\"
  }"

echo ""
echo ""
echo "✅ Done! Check PaymentAPI logs for details."
echo ""
echo "📊 To verify in MongoDB:"
echo "   - Check bill status should be 'Paid'"
echo "   - Check payment collection for new payment record"
