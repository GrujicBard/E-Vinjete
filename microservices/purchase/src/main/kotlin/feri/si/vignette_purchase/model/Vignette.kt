package feri.si.vignette_purchase.model

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document
import java.time.LocalDateTime

@Document("Vignettes")
data class Vignette(
    @Id
    var id: String?,
    val userId: String,
    val registration: String,
    val type: String,
    val dateCreated: String  = LocalDateTime.now().toString(),
    val dateValid: String
)