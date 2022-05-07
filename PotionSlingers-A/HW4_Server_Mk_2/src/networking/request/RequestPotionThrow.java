package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponsePotionThrow;
import utility.DataReader;
import core.NetworkManager;

public class RequestPotionThrow extends GameRequest {
    private int w, x, y, z;
    // Responses
    private ResponsePotionThrow responsePotionThrow;

    public RequestPotionThrow() {
        responses.add(responsePotionThrow = new ResponsePotionThrow());
    }

    @Override
    public void parse() throws IOException {
        w = DataReader.readInt(dataInput);
        x = DataReader.readInt(dataInput);
        y = DataReader.readInt(dataInput);
        z = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responsePotionThrow.setPlayer(player);
        responsePotionThrow.setData(w, x, y, z);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responsePotionThrow);
    }
}