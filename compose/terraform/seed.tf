﻿resource "vault_kv_secret_v2" "keys" {
  mount   = "secret" 
  name    = "secret"
  data_json = jsonencode({
    apiKeys = {
      external = [
        {
          serviceName = "exchangeRate"
          key         = "secret"
        }
      ]
      internal = [
        {
          serviceName = "currencies"
          key         = "6e7fc397ba51496ea82e31077cbb8374"
        },
        {
          serviceName = "customers"
          key         = "1a3032be2dec4030bd245e4f6fc51cb3"
        },
        {
          serviceName = "users"
          key         = "6f25bb953a9e4ae0a962b2d9b0dbfbd6"
        },
        {
          serviceName = "wallets"
          key         = "73ed82e2186240c8bfcbdd326c8abda4"
        },
        {
          serviceName = "notifications"
          key         = "c1d3b46474ad4d0684e7ae58fe39df8b"
        },
        {
          serviceName = "payments"
          key         = "7704ad46dcd9442e906e79b7d71f9742"
        }
      ]
    }
  })
}