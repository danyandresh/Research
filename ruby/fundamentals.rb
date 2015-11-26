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

def optional_params(p1= :a, p2 = :b, p3)#, p4 = :d)
    puts p3
end

optional_params(:c)

def keyword_args(p3= 1, p1: :a, p2: :b)
    puts "Keyword args " + p3.to_s
end

keyword_args(5, p2: 2)

def keyword_double_splat_args(p3= 1, p1: :a, **p2)
    puts "Double splat: "
    p2.each { |k, v| puts "k: #{k} v: #{v}" }
end

keyword_double_splat_args(5, p2: 2, p4: 4, p5: 5, p6: 6)

h = {a: :a, b: :b}
keyword_double_splat_args(6, h)

puts "Playing with operators"
class Operators
    attr_reader :capacity
    
    def initialize()
        @h = {}
        @capacity = 0
    end
    
    def [](index)
        @h[index]
    end
    
    def []=(index, value)
        @h[index] = value
    end
    
    def <=>(other)
        h <=> other.h
    end
    
    def +(capacity)
        @capacity += capacity
    end
    
    def +@
        @capacity += 1
    end
    
    def !
        puts "Hash destructed"
    end
end

op1 = Operators.new
op1[:d] = :c
puts op1[:d]

puts "Capacity #{op1.capacity}"
op1 + 10
puts "Capacity #{op1.capacity}"

+op1
puts "Capacity #{op1.capacity}"

if !op1
    puts "Operators class destructed"
end
    
puts "playing with method calls"
class ReceiverObject
    def m1
        puts "Method 1 called"
    end
    
    def m2
        puts "Method 2 called"
    end
end

h = {method1: :m1, method2: :m2}

method = :method1

receiver = ReceiverObject.new

receiver.__send__(h[method])

method = :method2

receiver.__send__(h[method])

puts self

