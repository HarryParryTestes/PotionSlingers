package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponsePotionThrow;
import utility.DataReader;
import core.NetworkManager;

public class RequestPotionThrow extends GameRequest {
    private int throwerId, cardPosition, targetId, damage;
    // Responses
    private ResponsePotionThrow responsePotionThrow;

    public RequestPotionThrow() {
        responses.add(responsePotionThrow = new ResponsePotionThrow());
    }

    @Override
    public void parse() throws IOException {
        throwerId = DataReader.readInt(dataInput);
        cardPosition = DataReader.readInt(dataInput);
        targetId = DataReader.readInt(dataInput);
        damage = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responsePotionThrow.setPlayer(player);
        responsePotionThrow.setData(throwerId, cardPosition, targetId, damage);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responsePotionThrow);
    }
}