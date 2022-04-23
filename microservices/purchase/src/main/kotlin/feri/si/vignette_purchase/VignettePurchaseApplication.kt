package feri.si.vignette_purchase

import VignetteAuth.Protos.ReturnId
import VignetteAuth.Protos.UserServiceGrpc
import VignetteAuth.Protos.VoidRequest
import io.grpc.ManagedChannelBuilder
import io.swagger.v3.oas.annotations.OpenAPIDefinition
import io.swagger.v3.oas.annotations.info.Info
import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication


@SpringBootApplication
@OpenAPIDefinition(info = Info(title = "Vignette Purchase", version = "1.0", description = "Documentation APIs v1.0"))
class VignettePurchaseApplication

fun main(args: Array<String>) {
    runApplication<VignettePurchaseApplication>(*args)

//    val channel = ManagedChannelBuilder.forAddress("localhost", 7220)
//        .usePlaintext()
//        .build();
//    val stub = UserServiceGrpc.newBlockingStub(channel);
//    val id = ReturnId.newBuilder().setId("625c53ff58199221f2bf2d90").build();
//    val response = stub.getUser(id);
//    val response = stub.getUsers(VoidRequest.newBuilder().build());
//    println(response);

//    channel.shutdown();

}
