#!/bin/bash
# Test webhook to verify payment bug fix

BILL_ID="148a4d2e-8ed5-4d16-abea-10d3974e288f"
PAYMENT_API_URL="http://localhost:7212"

echo "🧪 Testing Webhook Call..."
echo ""

RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "$PAYMENT_API_URL/api/Payment/webhook/sepay" \
  -H "Content-Type: application/json" \
  -d "{
    \"AccountNumber\": \"0123456789\",
    \"Amount\": 500000,
    \"Description\": \"Thanh toan hoa don $BILL_ID\",
    \"TransactionId\": \"TEST_$(date +%s)\",
    \"TransactionDate\": \"$(date '+%Y-%m-%d %H:%M:%S')\",
    \"BankBrandName\": \"Test Bank\"
  }")

HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
BODY=$(echo "$RESPONSE" | head -n-1)

echo "HTTP Status: $HTTP_CODE"
echo "Response: $BODY"
echo ""

if [ "$HTTP_CODE" = "200" ]; then
    echo "✅ Webhook call successful"
    echo ""
    echo "Checking PaymentAPI log..."
    tail -50 /tmp/paymentapi.log | grep -E "webhook|bill|Successfully marked"
else
    echo "❌ Webhook call failed"
fi
