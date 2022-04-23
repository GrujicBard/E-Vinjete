package feri.si.vignette_purchase.repository

import feri.si.vignette_purchase.model.Vignette
import org.springframework.boot.autoconfigure.security.oauth2.client.OAuth2ClientProperties.Registration
import org.springframework.data.mongodb.repository.ReactiveMongoRepository
import reactor.core.publisher.Flux
import reactor.core.publisher.Mono

interface VignetteRepository: ReactiveMongoRepository<Vignette, String>{
    fun findByUserId(userId: String): Flux<Vignette>
    fun findByRegistration(registration: String): Flux<Vignette>
}