#!/bin/sh

# Decrypt the file
# --batch to prevent interactive command
# --yes to assume "yes" for questions
gpg --quiet --batch --yes --decrypt --passphrase="$DEREKHONEYCUTT_APPINSIGHTS_PASSCODE" --output ./DerekHoneycutt.Frontend/main/appinsights.js ./DerekHoneycutt.Frontend/main/appinsights.js.gpg
