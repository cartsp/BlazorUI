#!/bin/bash
set -euo pipefail

echo "=== Building BlazorUI solution ==="
dotnet build BlazorUI.slnx --no-incremental

echo ""
echo "=== Running tests ==="
dotnet test BlazorUI.slnx --verbosity normal --no-build

echo ""
echo "=== Checking format ==="
dotnet format BlazorUI.slnx --verify-no-changes --no-build

echo ""
echo "=== All checks passed ==="
