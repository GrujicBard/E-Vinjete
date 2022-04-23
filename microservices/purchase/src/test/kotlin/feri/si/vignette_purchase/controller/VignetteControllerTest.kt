package feri.si.vignette_purchase.controller

import feri.si.vignette_purchase.model.Vignette
import feri.si.vignette_purchase.repository.VignetteRepository
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.mockito.Mockito
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.boot.test.autoconfigure.web.reactive.WebFluxTest
import org.springframework.boot.test.mock.mockito.MockBean
import org.springframework.http.MediaType
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.reactive.server.WebTestClient
import org.springframework.web.reactive.function.BodyInserters
import reactor.core.publisher.Flux
import reactor.core.publisher.Mono
import java.time.LocalDateTime


@ExtendWith(SpringExtension::class)
@WebFluxTest(controllers = [VignetteController::class])
class VignetteControllerTest {

    @MockBean
    lateinit var repository: VignetteRepository

    @Autowired
    lateinit var webTestClient: WebTestClient

    @Test
    fun testGetVignetteById() {
        val vignette = Vignette(
            "678",
            "5",
            "IT-098",
            "2A",
            "23/04/2022",
            "23/04/2023")

        Mockito
            .`when`(repository.findById("678"))
            .thenReturn(Mono.just(vignette))

        webTestClient.get()
            .uri("/v1/vignette/678")
            .accept(MediaType.APPLICATION_JSON)
            .exchange()
            .expectStatus().is2xxSuccessful
            .expectBody()
            .jsonPath("$.id").isEqualTo("678")
            .jsonPath("$.userId").isEqualTo("5")
            .jsonPath("$.registration").isEqualTo("IT-098")
            .jsonPath("$.type").isEqualTo("2A")
            .jsonPath("$.dateCreated").isEqualTo("23/04/2022")
            .jsonPath("$.dateValid").isEqualTo("23/04/2023");

        Mockito.verify(repository).findById("678");
    }

    @Test
    fun testGetVignettesByUserId() {
        val vignette1 = Vignette(
            "1",
            "2",
            "IT-345",
            "2A",
            "23/04/2022",
            "23/04/2023")

        val vignette2 = Vignette(
            "2",
            "2",
            "IT-433",
            "2A",
            "23/04/2022",
            "23/04/2023")

        val vignettes: List<Vignette> = listOf(vignette1, vignette2)

        val vignetteFlux: Flux<Vignette> = Flux.fromIterable<Vignette>(vignettes)

        Mockito
            .`when`(repository.findByUserId("2"))
            .thenReturn(vignetteFlux)

        webTestClient.get()
            .uri("/v1/vignette/user/2")
            .accept(MediaType.APPLICATION_JSON)
            .exchange()
            .expectStatus().is2xxSuccessful
            .expectBodyList(Vignette::class.java);

        Mockito.verify(repository).findByUserId("2");
    }

    @Test
    fun testGetVignettesByRegistration() {
        val vignette1 = Vignette(
            "5",
            "9",
            "IT-324",
            "2A",
            "23/04/2022",
            "23/04/2023")

        val vignette2 = Vignette(
            "6",
            "4",
            "IT-768",
            "2A",
            "23/04/2022",
            "23/04/2023")

        val vignettes: List<Vignette> = listOf(vignette1, vignette2)

        val vignetteFlux: Flux<Vignette> = Flux.fromIterable<Vignette>(vignettes)

        Mockito
            .`when`(repository.findByRegistration("IT-768"))
            .thenReturn(vignetteFlux)

        webTestClient.get()
            .uri("/v1/vignette/reg/IT-768")
            .accept(MediaType.APPLICATION_JSON)
            .exchange()
            .expectStatus().is2xxSuccessful
            .expectBodyList(Vignette::class.java);

        Mockito.verify(repository).findByRegistration("IT-768");
    }

    @Test
    fun testPostVignette() {
        val vignette = Vignette(
            "123",
            "456",
            "IT-678",
            "2A",
            LocalDateTime.now().toString(),
            "infinity")
        Mockito.`when`(repository.save<Vignette>(vignette)).thenReturn(Mono.just(vignette))

        webTestClient.post()
            .uri("/v1/vignette")
            .accept(MediaType.APPLICATION_JSON)
            .body(BodyInserters.fromValue(vignette))
            .exchange()
            .expectStatus().is2xxSuccessful

        Mockito.verify(repository).save(vignette);
    }

    @Test
    fun testDeleteVignette() {

        Mockito
            .`when`(repository.deleteById("2"))
            .thenReturn(Mono.empty())

        webTestClient.delete().uri("/v1/vignette/delete/2")
            .exchange()
            .expectStatus().isOk
    }
}