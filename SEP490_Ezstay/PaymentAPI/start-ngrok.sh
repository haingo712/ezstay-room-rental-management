#!/bin/bash

echo "🚀 Starting Ngrok for PaymentAPI"
echo "================================"
echo ""

# Check if PaymentAPI is running
if ! lsof -i :7212 > /dev/null 2>&1; then
    echo "⚠️  WARNING: PaymentAPI is not running on port 7212!"
    echo "    Please start PaymentAPI first:"
    echo "    cd /Users/Kì\ 9/V7/SEP490_Ezstay/PaymentAPI && dotnet run"
    echo ""
    read -p "Continue anyway? (y/n) " -n 1 -r
    echo ""
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
fi

echo "📡 Starting ngrok tunnel..."
echo "   Local: https://localhost:7212"
echo "   Remote: will be displayed below"
echo ""
echo "⚠️  IMPORTANT: Copy the HTTPS URL and update it in SePay webhook settings!"
echo ""

# Start ngrok
ngrok http https://localhost:7212

