package feri.si.vignette_purchase.controller

import feri.si.vignette_purchase.logging.Log
import feri.si.vignette_purchase.logging.LogType
import feri.si.vignette_purchase.model.Vignette
import feri.si.vignette_purchase.repository.VignetteRepository
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.web.bind.annotation.*
import reactor.core.publisher.Flux
import reactor.core.publisher.Mono


@RestController
@RequestMapping("v1/vignette")
class VignetteController(@Autowired val vignetteRepository: VignetteRepository) {

    @GetMapping
    fun getAll(): Flux<Vignette> {
        println(Log(LogType.INFO, "GET vignettes"))
        return vignetteRepository.findAll()
    }

    @GetMapping("{id}")
    fun getVignetteById(@PathVariable id: String): Mono<Vignette> {
        println(Log(LogType.INFO, "GET vignette by id"))
        return vignetteRepository.findById(id)
    }

    @GetMapping("user/{userId}")
    fun getVignetteByUserId(@PathVariable userId: String): Flux<Vignette> {
        println(Log(LogType.INFO, "GET vignettes by userId"))
        return vignetteRepository.findByUserId(userId)
    }

    @GetMapping("reg/{reg}")
    fun getVignetteByRegistration(@PathVariable reg: String): Flux<Vignette> {
        println(Log(LogType.INFO, "GET vignettes by registration"))
        return vignetteRepository.findByRegistration(reg)
    }

    @PostMapping
    fun saveVignette(@RequestBody vignette: Vignette): Mono<Vignette> {
        println(Log(LogType.INFO, "POST vignette"))
        return vignetteRepository.save(vignette)
    }

    @PutMapping("/update/{id}")
    fun updateVignette(@PathVariable id: String, @RequestBody vignette: Vignette): Mono<Vignette> {
        println(Log(LogType.INFO, "PUT vignette"))
        return vignetteRepository.findById(id).flatMap {
            vignette.id = it.id;
            vignetteRepository.save(vignette);
        }
    }

    @DeleteMapping("/delete/{id}")
    fun deleteVignette(@PathVariable id: String): Mono<Void> {
        println(Log(LogType.INFO, "DELETE vignette"))
        return vignetteRepository.deleteById(id)
    }

    @DeleteMapping("/delete")
    fun deleteAll(): Mono<Void> {
        println(Log(LogType.INFO, "DELETE vignettes"))
        return vignetteRepository.deleteAll()
    }
}