#!/bin/bash

MIGRATIONS_FOLDER="migrations"

# Check if NAME argument is provided
if [ -z "$1" ]; then
    echo "Usage: $0 <migration_name>"
    exit 1
fi

NAME=$1

echo "Verifying if folder ${MIGRATIONS_FOLDER} exists."

# Verify if folder exists if not create structure
if [ ! -d "$MIGRATIONS_FOLDER" ]; then
    echo "Folder ${MIGRATIONS_FOLDER} does not exist. Creating structure."
    mkdir migrations
    touch migrations/.migration_counter
    echo 1 > migrations/.migration_counter
fi

echo "Loading current counter value"
Counter=$(cat migrations/.migration_counter)

echo "Current Count is ${Counter}."

echo "Creating migration file with name ${NAME}."

# Create files
mkdir -p migrations/$(printf %04d $Counter)
touch migrations/$(printf %04d $Counter)/$(printf %04d $Counter)-$NAME.up.sql
touch migrations/$(printf %04d $Counter)/$(printf %04d $Counter)-$NAME.down.sql

# Add template data
CurrentDate=$(date +"%Y-%m-%d")
echo "--SQL UP ${Counter}-${NAME}: ${CurrentDate}" > migrations/$(printf %04d $Counter)/$(printf %04d $Counter)-$NAME.up.sql
echo -e "-- Created By: $(whoami)" >> migrations/$(printf %04d $Counter)/$(printf %04d $Counter)-$NAME.up.sql

echo "-- SQL DOWN ${Counter}-${NAME}: ${CurrentDate}" > migrations/$(printf %04d $Counter)/$(printf %04d $Counter)-$NAME.down.sql
echo -e "-- Created By: $(whoami)" >> migrations/$(printf %04d $Counter)/$(printf %04d $Counter)-$NAME.down.sql

echo "Script created successfully."

# Increment counter
echo "Incrementing counter"
echo $((Counter + 1)) > migrations/.migration_counter