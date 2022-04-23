package feri.si.vignette_purchase.logging

import java.time.LocalDateTime
import java.util.*

data class Log(
    val logType: LogType,
    val http_call: String
){
    private val correlation_id: UUID = UUID.randomUUID();
    private val timestamp: String = LocalDateTime.now().toString();
    private val url: String  = "https://localhost:8080";
    private val app_name: String = "VignettePurchase";
    override fun toString(): String = "$timestamp $logType $url CorrelationId: $correlation_id [$app_name] - <* $http_call *>";
}