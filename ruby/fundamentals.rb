puts 'Hello!'

lander_count = 10

result = nil

puts nil.class

puts result.nil?


a = " xyz "

puts a.strip

puts a

puts a.strip!

puts a

def double(val)
  val * 2
end

puts double("abc")

count =10
message = if count > 10
  "bigger than ten"
else
  "smaller than ten"
end

puts message

res = `time /t`

puts res

puts "Using system"
res = system "time /t"
puts res

class Spaceship
  def launch(destination)
    @destination = destination
    #go towards destination
  end
  
  def destination()
    @destination
  end
end

ship = Spaceship.new
ship.launch("Earth")
puts ship.inspect
p ship
puts ship.destination

puts case ship
    when Spaceship then "ship is Spaceship"
    else "ship is UFO"
end

# puts Encoding.public_methods.sort
puts "encoding: " + Encoding.default_internal.to_s
puts "abc".encoding

result = 1000
puts "Box count: #{result}"
puts "Box count: #{result + 10}"
puts %*"'*
puts %q*"'*
puts %Q*"'*

puts "heredoc begins"
puts <<-EOS
    test
        multiline
    text
EOS
puts "heredoc ends"

puts 'test string'['text'].nil?

a = 'test string'
puts a
puts a.object_id
a['test'] = 'text'
puts a
puts a.object_id

a = a + ' asd'
puts a
puts a.object_id

puts "%05d" % 23

h = { a: "a", b: "b" }
puts h




