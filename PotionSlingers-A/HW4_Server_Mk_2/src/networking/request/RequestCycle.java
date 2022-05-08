package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseCycle;
import utility.DataReader;
import core.NetworkManager;

public class RequestCycle extends GameRequest {
    private int x, y;
    // Responses
    private ResponseCycle responseCycle;

    public RequestCycle() {
        responses.add(responseCycle = new ResponseCycle());
    }

    @Override
    public void parse() throws IOException {
        x = DataReader.readInt(dataInput);
        y = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseCycle.setPlayer(player);
        responseCycle.setData(x, y);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseCycle);
    }
}