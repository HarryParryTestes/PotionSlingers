package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseBuy;
import utility.DataReader;
import core.NetworkManager;

public class RequestBuy extends GameRequest {
    private int x, y;
    // Responses
    private ResponseBuy responseBuy;

    public RequestBuy() {
        responses.add(responseBuy = new ResponseBuy());
    }

    @Override
    public void parse() throws IOException {
        x = DataReader.readInt(dataInput);
        y = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseBuy.setPlayer(player);
        responseBuy.setData(x, y);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseBuy);
    }
}