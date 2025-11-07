#!/bin/bash

echo "🚀 Starting PaymentAPI..."
echo "========================"
echo ""

cd "/Users/Kì 9/V7/SEP490_Ezstay/PaymentAPI"

# Build first
echo "📦 Building..."
dotnet build

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Build successful!"
    echo ""
    echo "🏃 Running PaymentAPI..."
    echo "========================"
    echo ""
    dotnet run
else
    echo ""
    echo "❌ Build failed!"
    exit 1
fi
