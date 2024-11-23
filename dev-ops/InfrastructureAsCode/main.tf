provider "azurerm" {
  features {}
}


resource "azurerm_resource_group" "rg" {
  name     = "CashflowV2"
  location = "East US"
}

resource "azurerm_container_group" "container_app" {
  name                = "container-app"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"

  container {
    name   = "dotnet-app"
    image  = "mcr.microsoft.com/dotnet/aspnet:9.0"
    cpu    = 1
    memory = 4

    ports {
      port     = 80
      protocol = "TCP"
    }

    environment_variables = {
      "DOTNET_ENVIRONMENT" = "Development"
    }
  }

  tags = {
    environment = "production"
  }
}

resource "azurerm_static_site" "static_web_app" {
  name                = "static-web-app"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku_tier            = "Free"
}

resource "azurerm_function_app" "function_app" {
  name                       = "function-app"
  location                   = azurerm_resource_group.rg.location
  resource_group_name        = azurerm_resource_group.rg.name
  app_service_plan_id        = azurerm_app_service_plan.plan.id
  storage_account_name       = azurerm_storage_account.storage.name
  storage_account_access_key = azurerm_storage_account.storage.primary_access_key
  version                    = "~3"
  os_type                    = "linux"
}

resource "azurerm_servicebus_namespace" "service_bus" {
  name                = "service-bus"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Standard"
}

resource "azurerm_sql_server" "sql_server" {
  name                         = "sql-server"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "adminuser"
  administrator_login_password = "H@Sh1CoR3!"
}

resource "azurerm_sql_database" "sql_database" {
  name                = "sql-database"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  server_name         = azurerm_sql_server.sql_server.name
  edition             = "Basic"
  requested_service_objective_name = "S0"
  max_size_bytes      = "2147483648" # 2 GB
}