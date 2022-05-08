package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseSell;
import utility.DataReader;
import core.NetworkManager;

public class RequestSell extends GameRequest {
    private int x, y;
    // Responses
    private ResponseSell responseSell;

    public RequestSell() {
        responses.add(responseSell = new ResponseSell());
    }

    @Override
    public void parse() throws IOException {
        x = DataReader.readInt(dataInput);
        y = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseSell.setPlayer(player);
        responseSell.setData(x, y);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseSell);
    }
}