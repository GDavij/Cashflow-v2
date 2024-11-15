MIGRATIONS_FOLDER := ./migrations

# Usage: make add_migration migration_name=your_migration_name
add_migration:
	./shell-scripts/sql-up.sh $(migration_name)