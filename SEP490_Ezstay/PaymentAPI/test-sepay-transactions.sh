#!/bin/bash

echo "🔍 Testing SePay API - Get Recent Transactions"
echo "=============================================="
echo ""

# SePay credentials from appsettings.json
SEPAY_URL="https://my.sepay.vn/userapi"
SECRET_KEY="88SQXILZRTFZPPNZA9HW9MW0XOCMOVXXTYEC75ICURGDKHBS3Y6QWONJKLZ1GCDF"
ACCOUNT_NUMBER="0799568616"

echo "📊 Fetching transactions for account: $ACCOUNT_NUMBER"
echo ""

# Get transactions from last 24 hours
FROM_DATE=$(date -u -v-24H '+%d/%m/%Y')
TO_DATE=$(date -u '+%d/%m/%Y')

echo "📅 Date range: $FROM_DATE - $TO_DATE"
echo ""

curl -s "${SEPAY_URL}/transactions?account_number=${ACCOUNT_NUMBER}&limit=50" \
  -H "Authorization: Bearer ${SECRET_KEY}" | head -100

echo ""
echo ""
echo "✅ Done! Check if there's any transaction with Bill ID in content"
